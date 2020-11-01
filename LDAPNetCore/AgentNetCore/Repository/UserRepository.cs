﻿using AgentNetCore.Application;
using AgentNetCore.Model;
using AgentNetCore.Repository;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace AgentNetCore.Context
{
    public class UserRepository
    {
        private readonly MySQLContext _mySQLContext;
        public UserRepository(MySQLContext mySQLContext)
        {
            _mySQLContext = mySQLContext;
        }

        #region CRUD
        public User Create(User user)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                ServerRepository serverRepo = new ServerRepository(_mySQLContext);
                string domain = serverRepo.ConvertToDomain(user.PathDomain);
                credential.Domain = domain;
                string pathDomain = serverRepo.GetPathByDN(user.PathDomain);
                DirectoryEntry dirEntry = new DirectoryEntry(pathDomain, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = ("samaccountname=" + user.SamAccountName);
                SearchResult result = search.FindOne();
                if (result == null)
                {
                    DirectoryEntry newUser = dirEntry.Children.Add("CN=" + user.Name, ObjectApplication.Category.user.ToString());
                    newUser.Properties["samAccountName"].Value = user.SamAccountName;
                    newUser.Properties["givenName"].Value = user.FirstName;
                    newUser.Properties["initials"].Value = user.Inicials;
                    newUser.Properties["c"].Value = user.Country;
                    newUser.Properties["cn"].Value = user.Name;
                    newUser.Properties["company"].Value = user.Company;
                    newUser.Properties["department"].Value = user.Departament;
                    newUser.Properties["description"].Value = user.Description;
                    newUser.Properties["displayName"].Value = (user.DisplayName + " - " + user.EmployeeID);
                    newUser.Properties["distinguishedName"].Value = "CN=" + user.DisplayName + "," + user.PathDomain;
                    newUser.Properties["employeeID"].Value = user.EmployeeID;
                    newUser.Properties["l"].Value = user.City;
                    newUser.Properties["mail"].Value = user.EmailAddress;
                    newUser.Properties["manager"].Value = "CN=" + user.Manager + "," + user.PathDomain; //"CN=Ghost Rider,CN=Users,DC=MarvelDomain,DC=local";//CN=Ghost Rider,OU=Users,OU=UnitedStates,OU=Marvel,OU=Marvel Company,DC=MarvelDomain,DC=local
                    newUser.Properties["mobile"].Value = user.MobilePhone;
                    newUser.Properties["name"].Value = user.Name;
                    newUser.Properties["o"].Value = user.Departament;
                    newUser.Properties["physicalDeliveryOfficeName"].Value = user.Office;
                    newUser.Properties["postalCode"].Value = user.PostalCode;
                    newUser.Properties["sn"].Value = user.Surname;
                    newUser.Properties["st"].Value = user.State;
                    newUser.Properties["streetAddress"].Value = user.StreetAddress;
                    newUser.Properties["telephoneNumber"].Value = user.OfficePhone;
                    newUser.Properties["title"].Value = user.Title;
                    newUser.Properties["userPrincipalName"].Value = user.EmailAddress;
                    newUser.CommitChanges();
                    SetEnable(newUser);
                    newUser.CommitChanges();
                    dirEntry.Close();
                    newUser.Close();
                    return FindBySamName(pathDomain, user.SamAccountName);
                }
                else
                {
                    Console.WriteLine("\r\nO Usuario ja existe no contexto:\r\n\t");
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                if (e.Message.Equals("The object already exists.\r\n"))
                {
                    Console.WriteLine("\r\nO Usuario ja existe no contexto:\r\n\t" + e.GetType() + ":" + e.Message);
                }
                return null;
            }
        }
        public User Create2(User user)
        {
            try
            {
                CredentialRepository connect = new CredentialRepository(_mySQLContext);//user.PathDomain, ObjectApplication.Category.user);
                connect.Domain = user.PathDomain;
                PrincipalContext pc = new PrincipalContext(ContextType.Domain, user.SamAccountName, user.PathDomain);
                UserPrincipal userPrincipal = new UserPrincipal(pc);
                userPrincipal.SamAccountName = user.SamAccountName;
                userPrincipal.GivenName = user.FirstName;
                userPrincipal.MiddleName = user.Name;
                userPrincipal.Surname = user.Name;
                userPrincipal.Description = user.Description;
                userPrincipal.DisplayName = user.DisplayName;
                userPrincipal.EmployeeId = user.EmployeeID;
                userPrincipal.PasswordNotRequired = user.PasswordNotRequired;
                userPrincipal.AccountExpirationDate = user.AccountExpirationDate;
                userPrincipal.Enabled = user.Enabled;
                userPrincipal.Save();
                user.DistinguishedName = userPrincipal.DistinguishedName;
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }
            return user;
        }
        public List<User> FindAll()
        {
            ConfigurationRepository config = new ConfigurationRepository(_mySQLContext);
            return FindAll(config.GetConfiguration("DefaultDomain"));
        }
        public List<User> FindAll(string domain)
        {
            try
            {
                CredentialRepository connect = new CredentialRepository(_mySQLContext);
                connect.Domain = domain;
                ServerRepository sr = new ServerRepository(_mySQLContext);
                DirectoryEntry dirEntry = new DirectoryEntry(sr.GetPathByServer(domain), connect.User, connect.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                List<User> userList = new List<User>();
                search.Filter = "(&(objectCategory=User)(objectClass=person))";
                var usersResult = search.FindAll();
                List<SearchResult> results = new List<SearchResult>();
                foreach (SearchResult userResult in usersResult)
                {
                    userList.Add(GetResult(userResult));
                }
                return userList;
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return new List<User>();
            }

        }
        private User FindOne(string domain, string campo, string valor, string[] fields)
        {
            try
            {
                CredentialRepository connect = new CredentialRepository(_mySQLContext);
                connect.Domain = domain;
                connect.Domain = ObjectApplication.Category.user.ToString();
                ServerRepository sr = new ServerRepository(_mySQLContext);
                DirectoryEntry dirEntry = new DirectoryEntry(sr.GetPathByServer(domain), connect.User, connect.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                User user = new User();
                search.Filter = "(" + campo + "=" + valor + ")";
                foreach (string atribute in fields)
                {
                    search.PropertiesToLoad.Add(atribute);
                }
                user = GetResult(search.FindOne());
                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        public User FindBySamName(string pathDomain, string samName)
        {// INPUT (PathDomain, SamAccountName) 
         // OUTPU (User)

            try
            {
                return FindOne(pathDomain, "samAccountName", samName);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        public User FindByEmail(string pathDomain, string email)
        {// INPUT (PathDomain, e-mail) 
         // OUTPU (User)
            try
            {
                return FindOne(pathDomain, "mail", email);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        public User FindByEmail(string email)
        {// INPUT (PathDomain, e-mail) 
         // OUTPU (User)
            User user = new User();
            ServerRepository serverRepo = new ServerRepository(_mySQLContext);
            string pathDomain = serverRepo.GetPathByServer(serverRepo.ConvertToDomain(email));
            user = FindByEmail(pathDomain, email);
            if (user.SamAccountName != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }
        private User FindOne(string pathDomain, string campo, string valor)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                ServerRepository sr = new ServerRepository(_mySQLContext);
                credential.Domain = sr.ConvertToDomain(pathDomain);
                DirectoryEntry dirEntry = new DirectoryEntry(pathDomain, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = "(" + campo + "=" + valor + ")";
                return GetResult(search.FindOne());
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }

        }
        public User FindByName(string pathDomain, string name)
        {
            try
            {
                return FindOne(pathDomain, "cn", name);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        public User Update(User user)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                ServerRepository serverRepo = new ServerRepository(_mySQLContext);
                string domain = serverRepo.ConvertToDomain(user.PathDomain);
                credential.Domain = domain;
                string pathDomain = serverRepo.GetPathByDN(user.PathDomain);
                DirectoryEntry dirEntry = new DirectoryEntry(pathDomain, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = ("samaccountname=" + user.SamAccountName);
                SearchResult result = search.FindOne();
                if (result != null)
                {
                    DirectoryEntry newUser = result.GetDirectoryEntry();
                    newUser.Properties["givenName"].Value = user.FirstName;
                    newUser.Properties["initials"].Value = user.Inicials;
                    newUser.Properties["c"].Value = user.Country;
                    newUser.Properties["company"].Value = user.Company;
                    newUser.Properties["department"].Value = user.Departament;
                    newUser.Properties["description"].Value = user.Description;
                    newUser.Properties["displayName"].Value = (user.DisplayName + " - " + user.EmployeeID);
                    newUser.Properties["employeeID"].Value = user.EmployeeID;
                    newUser.Properties["l"].Value = user.City;
                    newUser.Properties["mail"].Value = user.EmailAddress;
                    newUser.Properties["mobile"].Value = user.MobilePhone;
                    newUser.Properties["o"].Value = user.Departament;
                    newUser.Properties["physicalDeliveryOfficeName"].Value = user.Office;
                    newUser.Properties["postalCode"].Value = user.PostalCode;
                    newUser.Properties["sn"].Value = user.Surname;
                    newUser.Properties["st"].Value = user.State;
                    newUser.Properties["streetAddress"].Value = user.StreetAddress;
                    newUser.Properties["telephoneNumber"].Value = user.OfficePhone;
                    newUser.Properties["title"].Value = user.Title;
                    //newUser.Properties["manager"].Value = "CN=" + user.Manager + "," + user.PathDomain;
                    newUser.CommitChanges();
                    newUser.Rename("cn=" + user.Name);
                    newUser.CommitChanges();
                    SetEnable(newUser);
                    newUser.CommitChanges();
                    dirEntry.Close();
                    newUser.Close();
                    return FindBySamName(pathDomain, user.SamAccountName);
                }
                else
                {
                    Console.WriteLine("\r\nUser not identify:\r\n\t");
                    return user;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return user;
            }
        }
        public void Delete(User user)
        {
            try
            {
                ServerRepository serverRepo = new ServerRepository(_mySQLContext);
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                string domain = serverRepo.ConvertToDomain(user.PathDomain);
                credential.Domain = domain;
                string pathDomain = user.PathDomain;
                DirectoryEntry dirEntry = new DirectoryEntry(pathDomain, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = ("SamAccountName=" + user.SamAccountName);
                DirectoryEntry userFind = (search.FindOne()).GetDirectoryEntry();
                if (userFind != null)
                {
                    userFind.DeleteTree();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }
        }
        #endregion

        #region AUTH
        private bool Auth(string domainName, string userName, string password)
        {
            bool ret = false;

            try
            {
                DirectoryEntry de = new DirectoryEntry("LDAP://" + domainName, userName, password);
                DirectorySearcher dsearch = new DirectorySearcher(de);
                SearchResult results = null;

                results = dsearch.FindOne();

                ret = true;
            }
            catch
            {
                ret = false;
            }

            return ret;
        }
        #endregion

        #region GET
        private User GetProperties(User user, ResultPropertyCollection fields)
        {
            try
            {
                ServerRepository sr = new ServerRepository(_mySQLContext);
                foreach (String ldapField in fields.PropertyNames)
                {
                    foreach (Object myCollection in fields[ldapField])
                    {

                        switch (ldapField)
                        {
                            case "samaccountname":
                                user.SamAccountName = myCollection.ToString();
                                user.LogonName = myCollection.ToString();
                                break;
                            case "givenname":
                                user.FirstName = myCollection.ToString();
                                break;
                            case "initials":
                                user.Inicials = myCollection.ToString();
                                break;
                            case "c":
                                user.Country = myCollection.ToString();
                                break;
                            case "cn":
                                user.DisplayName = myCollection.ToString();
                                break;
                            case "company":
                                user.Company = myCollection.ToString();
                                break;
                            case "department":
                                user.Departament = myCollection.ToString();
                                break;
                            case "description":
                                user.Description = myCollection.ToString();
                                break;
                            case "displayName":
                                user.DisplayName = myCollection.ToString();
                                break;
                            case "distinguishedname":
                                user.DistinguishedName = myCollection.ToString();
                                break;
                            case "adspath":
                                user.PathDomain = myCollection.ToString();
                                user.Domain = sr.ConvertToDomain(myCollection.ToString());
                                break;
                            case "employeeid":
                                user.EmployeeID = myCollection.ToString();
                                break;
                            case "l":
                                user.City = myCollection.ToString();
                                break;
                            case "mail":
                                user.EmailAddress = myCollection.ToString();
                                break;
                            case "manager":
                                user.Manager = myCollection.ToString();
                                break;
                            case "mobile":
                                user.MobilePhone = myCollection.ToString();
                                break;
                            case "name":
                                user.Name = myCollection.ToString();
                                break;
                            case "o":
                                user.Departament = myCollection.ToString();
                                break;
                            case "physicaldeliveryofficename":
                                user.Office = myCollection.ToString();
                                break;
                            case "postalcode":
                                user.PostalCode = myCollection.ToString();
                                break;
                            case "sn":
                                user.Surname = myCollection.ToString();
                                break;
                            case "st":
                                user.State = myCollection.ToString();
                                break;
                            case "streetaddress":
                                user.StreetAddress = myCollection.ToString();
                                break;
                            case "telephonenumber":
                                user.OfficePhone = myCollection.ToString();
                                break;
                            case "title":
                                user.Title = myCollection.ToString();
                                break;
                            case "userprincipalname":
                                user.EmailAddress = myCollection.ToString();
                                break;
                        }
                        Console.WriteLine(String.Format("{0,-20} : {1}", ldapField, myCollection.ToString()));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return user;
            }
            return user;
        }
        private User GetResult(SearchResult result)
        {
            User user = new User();
            try
            {
                if (result != null)
                {
                    return GetProperties(user, result.Properties);
                }
                else
                {
                    Console.WriteLine("User not found!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);

            }
            return user;
        }
        private List<User> GetResultList(SearchResult result)
        {
            List<User> userList = new List<User>();
            if (result != null)
            {
                ResultPropertyCollection fields = result.Properties;

                for (int i = 0; i < fields.Count; i++)
                {
                    User user = new User();

                    userList.Add(GetProperties(user, fields));
                }

                return userList;
            }

            else
            {
                Console.WriteLine("User not found!");
                return userList;
            }
        }
        #endregion

        #region SET
        private void SetEnable(DirectoryEntry user)
        {
            try
            {
                var groupType = unchecked((int)(~UserAccountControl.ACCOUNTDISABLE));
                int userAccountControlValue = (int)user.Properties["userAccountControl"].Value;

                user.Properties["userAccountControl"].Value = userAccountControlValue & groupType;
                user.CommitChanges();
                user.Close();
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }
        }
        private void SetDisable(DirectoryEntry user)
        {
            try
            {
                int userAccountControlValue = (int)user.Properties["userAccountControl"].Value;
                user.Properties["userAccountControl"].Value = userAccountControlValue | ~0x2;
                user.CommitChanges();
                user.Close();
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }
        }
        #endregion
        #region User
        public List<User> UserChangeGroup(List<User> users, List<Group> newGroups, List<Group> oldGroups)
        {

            try
            {
                List<User> refreshUsers = new List<User>();

                foreach (var newGroup in newGroups)
                {
                    foreach (User user in users)
                    {
                        AddUserToGroup(user, newGroup);
                    }
                }
                foreach (var oldGroup in oldGroups)
                {
                    foreach (User user in users)
                    {
                        RemoveUserToGroup(user, oldGroup);
                    }
                }
                ServerRepository sr = new ServerRepository(_mySQLContext);
                foreach (var user in users)
                {
                    refreshUsers.Add(FindBySamName(sr.GetPathByDN(user.DistinguishedName), user.PathDomain));
                }
                return refreshUsers;
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }

        }
        public User UserChangeGroup(User user, Group newGroup, Group oldGroup)
        {
            try
            {
                AddUserToGroup(user, newGroup);
                RemoveUserToGroup(user, oldGroup);
                return FindByEmail(user.EmailAddress, user.PathDomain);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }

        }
        private void AddUserToGroup(User user, Group group)
        {
            try
            {
                CredentialRepository connect = new CredentialRepository(_mySQLContext);
                connect.Domain = user.Domain;
                connect.Domain = ObjectApplication.Category.user.ToString();
                ServerRepository sr = new ServerRepository(_mySQLContext);
                DirectoryEntry dirEntry = new DirectoryEntry(sr.GetPathByServer(user.PathDomain), connect.User, connect.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                PrincipalContext pc = new PrincipalContext(ContextType.Domain, user.SamAccountName, user.PathDomain);
                GroupPrincipal addGroup = GroupPrincipal.FindByIdentity(pc, group.SamAccountName);
                addGroup.Members.Add(pc, IdentityType.UserPrincipalName, user.SamAccountName);
                addGroup.Save();
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }
        }
        private void RemoveUserToGroup(User user, Group group)
        {
            try
            {
                CredentialRepository connect = new CredentialRepository(_mySQLContext);
                connect.Domain = user.PathDomain;
                ServerRepository sr = new ServerRepository(_mySQLContext);
                DirectoryEntry dirEntry = new DirectoryEntry(sr.GetPathByServer(user.PathDomain), connect.User, connect.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                PrincipalContext pc = new PrincipalContext(ContextType.Domain, user.SamAccountName, user.PathDomain);
                GroupPrincipal addGroup = GroupPrincipal.FindByIdentity(pc, group.SamAccountName);
                addGroup.Members.Remove(pc, IdentityType.UserPrincipalName, user.SamAccountName);
                addGroup.Save();
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);

            }
        }
        public bool ResetPassSamName(string domain, string samName, string newPass)
        {
            try
            {
                CredentialRepository connect = new CredentialRepository(_mySQLContext);//domain, ObjectApplication.Category.user);
                connect.Domain = domain;
                PrincipalContext pc = new PrincipalContext(ContextType.Domain, samName, domain);
                UserPrincipal userPrincipal = new UserPrincipal(pc);
                if (userPrincipal != null)
                {
                    ServerRepository sr = new ServerRepository(_mySQLContext);
                    DirectoryEntry dirEntry = new DirectoryEntry(sr.GetPathByServer(domain), connect.User, connect.Pass);
                    DirectorySearcher search = new DirectorySearcher(dirEntry);
                    search.Filter = ("SamAccountName=" + samName);
                    DirectoryEntry user = (search.FindOne()).GetDirectoryEntry();
                    user.Invoke("ChangePassword", new object[] { newPass });
                    user.CommitChanges();
                    user.Close();
                    dirEntry.Close();
                    return true;
                }
                else
                {
                    Console.WriteLine("\r\nUsuário não encontrado");
                    return false;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return false;
            }
        }
        #endregion

        #region ENUM
        [Flags()]
        public enum UserAccountControl : uint
        {//https://docs.microsoft.com/en-us/windows/win32/adschema/c-user
            // Lógica dos Binários
            // & - 1 And 1 = 1
            // & - 0 And 0 = 0
            // & - 1 And 0 = 0
            // 
            // | - 1 Ou 0 = 1
            // | - 1 Ou 1 = 1
            // | - 0 Ou 0 = 0
            // [SCRIPT, ]

            /// <summary>
            /// The logon script is executed. 
            ///</summary>
            SCRIPT = 0x00000001, //1

            /// <summary>
            /// The user account is disabled. 
            ///</summary>
            ACCOUNTDISABLE = 0x00000002,//2

            /// <summary>
            /// The home directory is required. 
            ///</summary>
            HOMEDIR_REQUIRED = 0x00000008,//8

            /// <summary>
            /// The account is currently locked out. 
            ///</summary>
            LOCKOUT = 0x00000010,//16

            /// <summary>
            /// No password is required. 
            ///</summary>
            PASSWD_NOTREQD = 0x00000020,//32

            /// <summary>
            /// The user cannot change the password. 
            ///</summary>
            /// <remarks>
            /// Note:  You cannot assign the permission settings of PASSWD_CANT_CHANGE by directly modifying the UserAccountControl attribute. 
            /// For more information and a code example that shows how to prevent a user from changing the password, see User Cannot Change Password.
            // </remarks>
            PASSWD_CANT_CHANGE = 0x00000040,//64

            /// <summary>
            /// The user can send an encrypted password. 
            ///</summary>
            ENCRYPTED_TEXT_PASSWORD_ALLOWED = 0x00000080,//128

            /// <summary>
            /// This is an account for users whose primary account is in another domain. This account provides user access to this domain, but not 
            /// to any domain that trusts this domain. Also known as a local user account. 
            ///</summary>
            TEMP_DUPLICATE_ACCOUNT = 0x00000100,//256

            /// <summary>
            /// This is a default account type that represents a typical user. 
            ///</summary>
            NORMAL_ACCOUNT = 0x00000200,//512

            /// <summary>
            /// This is a permit to trust account for a system domain that trusts other domains. 
            ///</summary>
            INTERDOMAIN_TRUST_ACCOUNT = 0x00000800,//2048

            /// <summary>
            /// This is a computer account for a computer that is a member of this domain. 
            ///</summary>
            WORKSTATION_TRUST_ACCOUNT = 0x00001000,//4096

            /// <summary>
            /// This is a computer account for a system backup domain controller that is a member of this domain. 
            ///</summary>
            SERVER_TRUST_ACCOUNT = 0x00002000,//8192

            /// <summary>
            /// Not used. 
            ///</summary>
            Unused1 = 0x00004000,

            /// <summary>
            /// Not used. 
            ///</summary>
            Unused2 = 0x00008000,

            /// <summary>
            /// The password for this account will never expire. 
            ///</summary>
            DONT_EXPIRE_PASSWD = 0x00010000,//65536

            /// <summary>
            /// This is an MNS logon account. 
            ///</summary>
            MNS_LOGON_ACCOUNT = 0x00020000,//131072

            /// <summary>
            /// The user must log on using a smart card. 
            ///</summary>
            SMARTCARD_REQUIRED = 0x00040000,//262144

            /// <summary>
            /// The service account (user or computer account), under which a service runs, is trusted for Kerberos delegation. Any such service 
            /// can impersonate a client requesting the service. 
            ///</summary>
            TRUSTED_FOR_DELEGATION = 0x00080000,//524288

            /// <summary>
            /// The security context of the user will not be delegated to a service even if the service account is set as trusted for Kerberos delegation. 
            ///</summary>
            NOT_DELEGATED = 0x00100000,//1048576

            /// <summary>
            /// Restrict this principal to use only Data Encryption Standard (DES) encryption types for keys. 
            ///</summary>
            USE_DES_KEY_ONLY = 0x00200000,//2097152

            /// <summary>
            /// This account does not require Kerberos pre-authentication for logon. 
            ///</summary>
            DONT_REQUIRE_PREAUTH = 0x00400000,//4194304

            /// <summary>
            /// The user password has expired. This flag is created by the system using data from the Pwd-Last-Set attribute and the domain policy. 
            ///</summary>
            PASSWORD_EXPIRED = 0x00800000,//8388608

            /// <summary>
            /// The account is enabled for delegation. This is a security-sensitive setting; accounts with this option enabled should be strictly 
            /// controlled. This setting enables a service running under the account to assume a client identity and authenticate as that user to 
            /// other remote servers on the network.
            ///</summary>
            TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION = 0x01000000,//16777216

            /// <summary>
            /// 
            /// </summary>
            PARTIAL_SECRETS_ACCOUNT = 0x04000000,//67108864

            /// <summary>
            /// 
            /// </summary>
            USE_AES_KEYS = 0x08000000
        }
        #endregion
    }
}

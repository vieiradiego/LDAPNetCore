﻿using AgentNetCore.Model;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AgentNetCore.Context
{
    public class LDAPUser
    {
        private LDAPConnect _connect;
        private DirectoryEntry _dirEntry;
        private UserPrincipal _userPrincipal;
        private DirectorySearcher _search;
        public LDAPUser(string domain)
        {
            _connect = new LDAPConnect(domain, LDAPConnect.ObjectCategory.user);
            _dirEntry = new DirectoryEntry(_connect.Path, _connect.User, _connect.Pass);
            _search = new DirectorySearcher(_dirEntry);
            
        }

        #region CRUD
        public User Create(User user)
        {
            try
            {
                return SetProperties(user);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                if (e.Message.Equals("The object already exists.\r\n"))
                {

                    Console.WriteLine("\r\nO Usuario ja existe no contexto:\r\n\t" + e.GetType() + ":" + e.Message);
                }
                return user;
            }
        }
        public User Create2(User user)
        {
            try
            {
                _userPrincipal = new UserPrincipal(_connect.Context);
                _userPrincipal.SamAccountName = user.SamAccountName;
                _userPrincipal.GivenName = user.FirstName;
                _userPrincipal.MiddleName = user.Name;
                _userPrincipal.Surname = user.Name;
                _userPrincipal.Description = user.Description;
                _userPrincipal.DisplayName = user.DisplayName;
                _userPrincipal.EmployeeId = user.EmployeeID;
                _userPrincipal.PasswordNotRequired = user.PasswordNotRequired;
                _userPrincipal.AccountExpirationDate = user.AccountExpirationDate;
                _userPrincipal.Enabled = user.Enabled;
                _userPrincipal.Save();
                user.DistinguishedName = _userPrincipal.DistinguishedName;

            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }
            return user;
        }
        public List<User> FindAll(string campo, string valor)
        {
            try
            {
                List<User> userList = new List<User>();
                _search.Filter = "(&(objectCategory=User)(objectClass=person))";
                var usersResult = _search.FindAll();
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
        private User FindOne(string campo, string valor)
        {
            try
            {
                User user = new User();
                _search.Filter = "(" + campo + "=" + valor + ")";
                user = GetResult(_search.FindOne());
                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }

        }
        private User FindOne(string campo, string valor, string[] fields)
        {
            try
            {

                User user = new User();
                _search.Filter = "(" + campo + "=" + valor + ")";
                foreach (string atribute in fields)
                {
                    _search.PropertiesToLoad.Add(atribute);
                }
                user = GetResult(_search.FindOne());
                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        public User FindByEmail(string email)
        {
            try
            {
                return FindOne("mail", email);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        public User FindByName(string name)
        {
            try
            {
                return FindOne("cn", name);
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
                _userPrincipal = UserPrincipal.FindByIdentity(_connect.Context, user.SamAccountName);
                if (_userPrincipal != null)
                {
                    return UpdateUser(user);
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
                _userPrincipal = UserPrincipal.FindByIdentity(_connect.Context, user.DistinguishedName);
                if (_userPrincipal != null)
                {
                    _userPrincipal.Delete();
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
                    ResultPropertyCollection fields = result.Properties;
                    return GetProperties(user, fields);
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
        private User SetProperties(User user)
        {
            DirectoryEntry newUser = _dirEntry.Children.Add("CN=" + user.Name, "user");
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
            newUser.Properties["manager"].Value = "CN=" + user.Manager + "," + user.PathDomain;
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
            _dirEntry.Close();
            newUser.Close();
            return FindByEmail(user.EmailAddress);
        }
        private User UpdateUser(User user)
        {
            _search.Filter = ("SamAccountName=" + user.SamAccountName);
            DirectoryEntry newUser = (_search.FindOne()).GetDirectoryEntry();
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
            newUser.Properties["manager"].Value = "CN=" + user.Manager + "," + user.PathDomain;
            newUser.CommitChanges();
            newUser.Rename("cn=" + user.Name);
            newUser.CommitChanges();
            SetEnable(newUser);
            newUser.CommitChanges();
            _dirEntry.Close();
            newUser.Close();
            return FindByEmail(user.EmailAddress);
        }
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

        public void UserChangeGroup(User user, Group oldGroup, Group newGroup)
        {
            AddGroupToUser(user, newGroup);
            RemoveGroupToUser(user, oldGroup);
        }

        public void AddGroupToUser(List<User> userList, Group group)
        {
            foreach (var user in userList)
            {
                AddGroupToUser(user, group);
            }
        }

        public void AddGroupToUser(User samUser, Group samGroup)
        {
            try
            {
                GroupPrincipal addGroup = GroupPrincipal.FindByIdentity(_connect.Context, samGroup.SamAccountName);
                addGroup.Members.Add(_connect.Context, IdentityType.UserPrincipalName, samUser.SamAccountName);
                addGroup.Save();
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }
        }

        public void RemoveGroupToUser(User samUser, Group samGroup)
        {
            try
            {
                GroupPrincipal addGroup = GroupPrincipal.FindByIdentity(_connect.Context, samGroup.SamAccountName);
                addGroup.Members.Remove(_connect.Context, IdentityType.UserPrincipalName, samUser.SamAccountName);
                addGroup.Save();
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);

            }
        }
        private bool ResetPassByEmail(string email)
        {
            try
            {
                SearchResult _searchResult = _search.FindOne();
                if (_searchResult != null)
                {
                    _dirEntry.Invoke("ChangePassword", new object[] { "Batman2000." });
                    _dirEntry.CommitChanges();
                    _dirEntry.Close();
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

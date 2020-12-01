using AgentNetCore.Application;
using AgentNetCore.Model;
using AgentNetCore.Repository;
using AgentNetCore.Repository.Interface;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace AgentNetCore.Context
{
    public class UserRepository : IUserRepository
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
                credential.DN = user.DistinguishedName;
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = ("samAccountName=" + user.SamAccountName);
                SearchResult result = search.FindOne();
                if (result == null)
                {
                    DirectoryEntry newUser = dirEntry.Children.Add("CN=" + user.Name, ObjectApplication.Category.user.ToString());
                    if (!String.IsNullOrWhiteSpace(user.SamAccountName))
                    {
                        newUser.Properties["samAccountName"].Value = user.SamAccountName;
                        newUser.Properties["info"].Value = "Usuário criado pela API RESTful  - " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                    }
                    if (!String.IsNullOrWhiteSpace(user.DistinguishedName) && (!String.IsNullOrWhiteSpace(user.DisplayName)))
                    {
                        newUser.Properties["distinguishedName"].Value = "CN=" + user.DisplayName + "," + user.DistinguishedName;
                    }
                    if (!String.IsNullOrWhiteSpace(user.Name))
                    {
                        newUser.Properties["name"].Value = user.Name;
                        newUser.Properties["cn"].Value = user.Name;
                    }
                    if (!String.IsNullOrWhiteSpace(user.FirstName))
                    {
                        newUser.Properties["givenName"].Value = user.FirstName;
                    }
                    if (!String.IsNullOrWhiteSpace(user.Surname))
                    {
                        newUser.Properties["sn"].Value = user.Surname;
                    }

                    if (!String.IsNullOrWhiteSpace(user.Inicials))
                    {
                        newUser.Properties["initials"].Value = user.Inicials;
                    }
                    if (!String.IsNullOrWhiteSpace(user.DisplayName))
                    {
                        newUser.Properties["displayName"].Value = (user.DisplayName + " - " + user.EmployeeID);
                    }
                    if (!String.IsNullOrWhiteSpace(user.Title))
                    {
                        newUser.Properties["title"].Value = user.Title;

                    }
                    if (!String.IsNullOrWhiteSpace(user.EmailAddress))
                    {
                        newUser.Properties["mail"].Value = user.EmailAddress;
                        newUser.Properties["userPrincipalName"].Value = user.EmailAddress;
                    }
                    if (!String.IsNullOrWhiteSpace(user.Country))
                    {
                        newUser.Properties["c"].Value = user.Country;
                    }
                    if (!String.IsNullOrWhiteSpace(user.Description))
                    {
                        newUser.Properties["description"].Value = user.Description;
                    }
                    if (!String.IsNullOrWhiteSpace(user.Company))
                    {
                        newUser.Properties["company"].Value = user.Company;
                    }
                    if (!String.IsNullOrWhiteSpace(user.Departament))
                    {
                        newUser.Properties["department"].Value = user.Departament;
                        newUser.Properties["o"].Value = user.Departament;
                    }
                    if (!String.IsNullOrWhiteSpace(user.MobilePhone))
                    {
                        newUser.Properties["mobile"].Value = user.MobilePhone;
                    }
                    if (!String.IsNullOrWhiteSpace(user.OfficePhone))
                    {
                        newUser.Properties["telephoneNumber"].Value = user.OfficePhone;
                    }
                    if (!String.IsNullOrWhiteSpace(user.Manager))
                    {
                        newUser.Properties["manager"].Value = user.Manager;
                    }
                    if (!String.IsNullOrWhiteSpace(user.EmployeeID))
                    {
                        newUser.Properties["employeeID"].Value = user.EmployeeID;
                    }
                    if (!String.IsNullOrWhiteSpace(user.Office))
                    {
                        newUser.Properties["physicalDeliveryOfficeName"].Value = user.Office;
                    }
                    if (!String.IsNullOrWhiteSpace(user.State))
                    {
                        newUser.Properties["st"].Value = user.State;
                    }
                    if (!String.IsNullOrWhiteSpace(user.City))
                    {
                        newUser.Properties["l"].Value = user.City;
                    }
                    if (!String.IsNullOrWhiteSpace(user.StreetAddress))
                    {
                        newUser.Properties["streetAddress"].Value = user.StreetAddress;
                    }
                    if (!String.IsNullOrWhiteSpace(user.PostalCode))
                    {
                        newUser.Properties["postalCode"].Value = user.PostalCode;
                    }
                    newUser.CommitChanges();
                    if (user.Enabled)
                    {// 0 ou 1
                        SetEnable(newUser);
                        newUser.CommitChanges();
                    }
                    if (user.PasswordNotRequired)
                    {

                    }
                    if (user.PasswordNeverExpires)
                    {

                    }
                    if (user.CanNotChangePassword)
                    {

                    }
                    if (user.ChangePasswordAtLogon)
                    {

                    }
                    ResetPassBySamName(credential, user.SamAccountName, user.AccountPassword);
                    dirEntry.Close();
                    newUser.Close();
                    return FindBySamName(credential, user.SamAccountName);
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
                    Console.WriteLine("\r\nO Usuario já existe no contexto:\r\n\t" + e.GetType() + ":" + e.Message);
                }
                if (e.Message.Equals("The server is not operational.\r\n"))
                {
                    Console.WriteLine("\r\nServidor não operacional:\r\n\t" + e.GetType() + ":" + e.Message);
                }
                return null;
            }
        }
        public List<User> FindAll()
        {
            ConfigurationRepository config = new ConfigurationRepository(_mySQLContext);
            CredentialRepository credential = new CredentialRepository(_mySQLContext);
            credential.DN = config.GetConfiguration("DefaultDN");
            return FindAll(credential);
        }
        private List<User> FindAll(CredentialRepository credential)
        {
            try
            {
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
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
        private User FindOne(CredentialRepository credential, string campo, string valor)
        {
            try
            {
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = "(" + campo + "=" + valor + ")";
                var user = GetResult(search.FindOne());
                if ((user.DistinguishedName == null) && (user.SamAccountName == null)) return null;
                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }

        }
        public User FindByEmail(string dn, string email)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = dn;
                return FindOne(credential, "mail", email);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        public User FindBySamName(CredentialRepository credential, string samName)
        {
            try
            {
                var user = FindOne(credential, "samAccountName", samName);
                if (user != null)
                {
                    return user;
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        public User FindBySamName(string dn, string samName)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = dn;
                return FindOne(credential, "samAccountName", samName);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        public User FindByName(string dn, string name)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = dn;
                return FindOne(credential, "name", name);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        public User FindByFirstName(string dn, string firstName)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = dn;
                return FindOne(credential, "givenName", firstName);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }

        }
        public User FindByLastName(string dn, string lastName)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = dn;
                return FindOne(credential, "sn", lastName);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }

        public List<User> FindByDn(string dn)
        {
            CredentialRepository credential = new CredentialRepository(_mySQLContext);
            credential.DN = dn;
            return FindAll(credential);
        }

        public User FindByDnOne(string dn)
        {
            CredentialRepository credential = new CredentialRepository(_mySQLContext);
            credential.DN = dn;
            List<User> users = FindAll(credential);
            User user = (User)users[0];
            return user;
        }


        public User Update(User user)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = user.DistinguishedName;
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = ("samaccountname=" + user.SamAccountName);
                SearchResult result = search.FindOne();
                if (result != null)
                {
                    DirectoryEntry newUser = result.GetDirectoryEntry();
                    newUser.Properties["info"].Value = "Usuário Editado pela API RESTful  - " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                    if (!String.IsNullOrWhiteSpace(user.FirstName))
                    {
                        newUser.Properties["givenName"].Value = user.FirstName;
                    }
                    if (!String.IsNullOrWhiteSpace(user.Surname))
                    {
                        newUser.Properties["sn"].Value = user.Surname;
                    }
                    if (!String.IsNullOrWhiteSpace(user.Inicials))
                    {
                        newUser.Properties["initials"].Value = user.Inicials;
                    }
                    if (!String.IsNullOrWhiteSpace(user.DisplayName))
                    {
                        newUser.Properties["displayName"].Value = (user.DisplayName + " - " + user.EmployeeID);
                    }
                    if (!String.IsNullOrWhiteSpace(user.Title))
                    {
                        newUser.Properties["title"].Value = user.Title;
                    }
                    if (!String.IsNullOrWhiteSpace(user.EmailAddress))
                    {
                        newUser.Properties["mail"].Value = user.EmailAddress;
                        newUser.Properties["userPrincipalName"].Value = user.EmailAddress;
                    }
                    if (!String.IsNullOrWhiteSpace(user.Country))
                    {
                        newUser.Properties["c"].Value = user.Country;
                    }
                    if (!String.IsNullOrWhiteSpace(user.Description))
                    {
                        newUser.Properties["description"].Value = user.Description;
                    }
                    if (!String.IsNullOrWhiteSpace(user.Company))
                    {
                        newUser.Properties["company"].Value = user.Company;
                    }
                    if (!String.IsNullOrWhiteSpace(user.Departament))
                    {
                        newUser.Properties["department"].Value = user.Departament;
                        newUser.Properties["o"].Value = user.Departament;
                    }
                    if (!String.IsNullOrWhiteSpace(user.MobilePhone))
                    {
                        newUser.Properties["mobile"].Value = user.MobilePhone;
                    }
                    if (!String.IsNullOrWhiteSpace(user.OfficePhone))
                    {
                        newUser.Properties["telephoneNumber"].Value = user.OfficePhone;
                    }
                    if (!String.IsNullOrWhiteSpace(user.Manager))
                    {
                        newUser.Properties["manager"].Value = user.Manager;
                    }
                    if (!String.IsNullOrWhiteSpace(user.EmployeeID))
                    {
                        newUser.Properties["employeeID"].Value = user.EmployeeID;
                    }
                    if (!String.IsNullOrWhiteSpace(user.Office))
                    {
                        newUser.Properties["physicalDeliveryOfficeName"].Value = user.Office;
                    }
                    if (!String.IsNullOrWhiteSpace(user.State))
                    {
                        newUser.Properties["st"].Value = user.State;
                    }
                    if (!String.IsNullOrWhiteSpace(user.City))
                    {
                        newUser.Properties["l"].Value = user.City;
                    }
                    if (!String.IsNullOrWhiteSpace(user.StreetAddress))
                    {
                        newUser.Properties["streetAddress"].Value = user.StreetAddress;
                    }
                    if (!String.IsNullOrWhiteSpace(user.PostalCode))
                    {
                        newUser.Properties["postalCode"].Value = user.PostalCode;
                    }
                    newUser.CommitChanges();
                    if (user.Enabled)
                    {// 0 ou 1
                        SetEnable(newUser);
                    }
                    if (user.PasswordNotRequired)
                    {

                    }
                    if (user.PasswordNeverExpires)
                    {

                    }
                    if (user.CanNotChangePassword)
                    {

                    }
                    if (user.ChangePasswordAtLogon)
                    {

                    }
                    newUser.CommitChanges();
                    if (!String.IsNullOrWhiteSpace(user.Name))
                    {
                        newUser.Rename("cn=" + user.Name);
                        newUser.CommitChanges();
                    }
                    dirEntry.Close();
                    newUser.Close();
                    return FindBySamName(credential, user.SamAccountName);
                }
                else
                {
                    Console.WriteLine("\r\nUser not identify:\r\n\t");
                    return null;
                }
            }
            catch (Exception e)
            {
                if (e.Message.Equals("The object already exists."))
                {
                    return null;
                }
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        private User Disable(string dn, string samName)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = dn;
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = ("samaccountname=" + samName);
                SearchResult result = search.FindOne();
                if (result != null)
                {
                    DirectoryEntry newUser = result.GetDirectoryEntry();
                    SetDisable(newUser);
                    newUser.CommitChanges();
                    dirEntry.Close();
                    newUser.Close();
                    return FindBySamName(credential, samName);
                }
                else
                {
                    Console.WriteLine("\r\nUser not identify:\r\n\t");
                    return null;
                }
            }
            catch (Exception e)
            {
                if (e.Message.Equals("The object already exists."))
                {
                    return null;
                }
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }

        public User Disable(CredentialRepository credential, string samName)
        {
            try
            {
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = ("samaccountname=" + samName);
                SearchResult result = search.FindOne();
                if (result != null)
                {
                    DirectoryEntry newUser = result.GetDirectoryEntry();
                    SetDisable(newUser);
                    newUser.CommitChanges();
                    dirEntry.Close();
                    newUser.Close();
                    return FindBySamName(credential, samName);
                }
                else
                {
                    Console.WriteLine("\r\nUser not identify:\r\n\t");
                    return null;
                }
            }
            catch (Exception e)
            {
                if (e.Message.Equals("The object already exists."))
                {
                    return null;
                }
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        private User Enable(string dn, string samName)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = dn;
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = ("samaccountname=" + samName);
                SearchResult result = search.FindOne();
                if (result != null)
                {
                    DirectoryEntry newUser = result.GetDirectoryEntry();
                    SetEnable(newUser);
                    newUser.CommitChanges();
                    dirEntry.Close();
                    newUser.Close();
                    return FindBySamName(credential, samName);
                }
                else
                {
                    Console.WriteLine("\r\nUser not identify:\r\n\t");
                    return null;
                }
            }
            catch (Exception e)
            {
                if (e.Message.Equals("The object already exists."))
                {
                    return null;
                }
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        public User Enable(CredentialRepository credential, string samName)
        {
            try
            {
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = ("samaccountname=" + samName);
                SearchResult result = search.FindOne();
                if (result != null)
                {
                    DirectoryEntry newUser = result.GetDirectoryEntry();
                    SetEnable(newUser);
                    newUser.CommitChanges();
                    dirEntry.Close();
                    newUser.Close();
                    return FindBySamName(credential, samName);
                }
                else
                {
                    Console.WriteLine("\r\nUser not identify:\r\n\t");
                    return null;
                }
            }
            catch (Exception e)
            {
                if (e.Message.Equals("The object already exists."))
                {
                    return null;
                }
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        public void Delete(CredentialRepository credential, User user)
        {
            try
            {
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
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
        #region GET

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
                                user.UserAccountControl = myCollection.ToString();
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
                var groupType = unchecked((int)(UserAccountControl.ACCOUNTDISABLE));
                int userAccountControlValue = (int)user.Properties["userAccountControl"].Value;
                user.Properties["userAccountControl"].Value = userAccountControlValue | groupType;
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

        public List<User> ChangeGroupByUsers(string userDn, string oldGroupDn, string newGroupDn)
        {
            GroupRepository oldGroupRep = new GroupRepository(_mySQLContext);
            GroupRepository newGroupRep = new GroupRepository(_mySQLContext);
            List<User> users = ChangeGroup(FindByDn(userDn), (oldGroupRep.FindByDn(oldGroupDn)), (newGroupRep.FindByDn(newGroupDn)));
            return users;
        }
        private List<User> ChangeGroup(List<User> users, List<Group> newGroups, List<Group> oldGroups)
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
                foreach (var user in users)
                {
                    refreshUsers.Add(FindBySamName(user.DistinguishedName, user.SamAccountName));
                }
                return refreshUsers;
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }

        }
        public User ChangeGroupByUser(string userDn, string oldGroupDn, string newGroupDn)
        {
            GroupRepository oldGroupRep = new GroupRepository(_mySQLContext);
            GroupRepository newGroupRep = new GroupRepository(_mySQLContext);
            Group oldG = oldGroupRep.FindByDnOne(oldGroupDn);
            Group newG = newGroupRep.FindByDnOne(newGroupDn);
            User user = FindByDnOne(userDn);
            return (User)ChangeGroup(user, oldG, newG);
        }
        public User ChangeGroup(User user, Group newGroup, Group oldGroup)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = user.DistinguishedName;
                AddUserToGroup(user, newGroup);
                RemoveUserToGroup(user, oldGroup);
                return FindBySamName(credential, user.SamAccountName);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }

        }
        public void AddUserToGroup(string userDn, string groupDn)
        {
            GroupRepository groupRep = new GroupRepository(_mySQLContext);
            Group group = groupRep.FindByDnOne(groupDn);
            User user = FindByDnOne(userDn);
            AddUserToGroup(user, group);
        }
        private void AddUserToGroup(User user, Group group)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = user.DistinguishedName;
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                PrincipalContext pc = new PrincipalContext(ContextType.Domain, user.SamAccountName, credential.Path);
                GroupPrincipal addGroup = GroupPrincipal.FindByIdentity(pc, group.SamAccountName);
                addGroup.Members.Add(pc, IdentityType.UserPrincipalName, user.SamAccountName);
                addGroup.Save();
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }
        }
        public void RemoveUserToGroup(string userDn, string groupDn)
        {
            GroupRepository groupRep = new GroupRepository(_mySQLContext);
            Group group = groupRep.FindByDnOne(groupDn);
            User user = FindByDnOne(userDn);
            RemoveUserToGroup(user, group);
        }
        private void RemoveUserToGroup(User user, Group group)
        {
            try
            {
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = user.DistinguishedName;
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                PrincipalContext pc = new PrincipalContext(ContextType.Domain, user.SamAccountName, credential.Path);
                GroupPrincipal addGroup = GroupPrincipal.FindByIdentity(pc, group.SamAccountName);
                addGroup.Members.Remove(pc, IdentityType.UserPrincipalName, user.SamAccountName);
                addGroup.Save();
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);

            }
        }
        public bool ResetPassBySamName(CredentialRepository credential, string samName, string newPass)
        {
            try
            {
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = ("samaccountname=" + samName);
                SearchResult result = search.FindOne();
                if (result != null)
                {
                    DirectoryEntry user = result.GetDirectoryEntry();
                    user.Password = newPass;
                    user.CommitChanges();
                    dirEntry.Close();
                    user.Close();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return false;
            }
        }
        public List<Group> GetGroups(CredentialRepository credential, string userDn)
        {
            try
            {
                DirectoryEntry dirEntry = new DirectoryEntry(credential.Path, credential.User, credential.Pass);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = ("distinguishedName=" + userDn);
                SearchResult result = search.FindOne();
                if (result != null)
                {
                    GroupRepository groupRepo = new GroupRepository(_mySQLContext);
                    List<Group> groupList = new List<Group>();
                    Group group = new Group();
                    for (int i = 0; i < result.Properties["memberOf"].Count; i++)
                    {
                        group = groupRepo.FindByDnOne((String)result.Properties["memberOf"][i]);
                        groupList.Add(group);
                    }
                    return groupList;
                }
                else
                {
                    return null;
                }
            }
            catch (DirectoryServicesCOMException e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
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

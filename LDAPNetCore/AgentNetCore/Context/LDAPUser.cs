using AgentNetCore.Model;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
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
        public LDAPUser()
        {
            _connect = new LDAPConnect("users", "marveldomain.local", "192.168.0.99", false);
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
                _search.Filter = "(" + campo + "=" + valor + ")";
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
        private User FindOne(string campo, string valor, string [] fields)
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
        public User FindByName(string name, string[] fields)
        {
            try
            {
                
            }

            catch (Exception e)
            {
                Console.WriteLine("Exception caught:\n\n" + e.ToString());
            }
            return null;
        }
        public User Update(User user)
        {
            try
            {
                _userPrincipal = UserPrincipal.FindByIdentity(_connect.Context, user.EmailAddress);
                if (_userPrincipal != null)
                {
                    return SetProperties(user);
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
                _userPrincipal = UserPrincipal.FindByIdentity(_connect.Context, user.EmailAddress);
                if (_userPrincipal != null)
                {
                    _userPrincipal.Delete();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);

                if (e.Message == "The server is not operational")
                {//The server is not operational.

                }
            }
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
                                user.Name = myCollection.ToString();
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
            newUser.Properties["cn"].Value = user.DisplayName;
            newUser.Properties["company"].Value = user.Company;
            newUser.Properties["department"].Value = user.Departament;
            newUser.Properties["description"].Value = user.Description;
            newUser.Properties["displayName"].Value = user.DisplayName;
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
            newUser.Properties["sn"].Value = user.Name;
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
        private void SetEnable(DirectoryEntry user)
        {
            try
            {
                int userAccountControlValue = (int)user.Properties["userAccountControl"].Value;
                user.Properties["userAccountControl"].Value = userAccountControlValue & ~0x2;

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
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }
            return true;
        }
        #endregion

        #region ENUM
        [Flags()]
        public enum UserAccountControl : int
        {
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
            DONT_EXPIRE_PASSWD = 0x00010000,

            /// <summary>
            /// This is an MNS logon account. 
            ///</summary>
            MNS_LOGON_ACCOUNT = 0x00020000,

            /// <summary>
            /// The user must log on using a smart card. 
            ///</summary>
            SMARTCARD_REQUIRED = 0x00040000,

            /// <summary>
            /// The service account (user or computer account), under which a service runs, is trusted for Kerberos delegation. Any such service 
            /// can impersonate a client requesting the service. 
            ///</summary>
            TRUSTED_FOR_DELEGATION = 0x00080000,

            /// <summary>
            /// The security context of the user will not be delegated to a service even if the service account is set as trusted for Kerberos delegation. 
            ///</summary>
            NOT_DELEGATED = 0x00100000,

            /// <summary>
            /// Restrict this principal to use only Data Encryption Standard (DES) encryption types for keys. 
            ///</summary>
            USE_DES_KEY_ONLY = 0x00200000,

            /// <summary>
            /// This account does not require Kerberos pre-authentication for logon. 
            ///</summary>
            DONT_REQUIRE_PREAUTH = 0x00400000,

            /// <summary>
            /// The user password has expired. This flag is created by the system using data from the Pwd-Last-Set attribute and the domain policy. 
            ///</summary>
            PASSWORD_EXPIRED = 0x00800000,

            /// <summary>
            /// The account is enabled for delegation. This is a security-sensitive setting; accounts with this option enabled should be strictly 
            /// controlled. This setting enables a service running under the account to assume a client identity and authenticate as that user to 
            /// other remote servers on the network.
            ///</summary>
            TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION = 0x01000000,

            /// <summary>
            /// 
            /// </summary>
            PARTIAL_SECRETS_ACCOUNT = 0x04000000,

            /// <summary>
            /// 
            /// </summary>
            USE_AES_KEYS = 0x08000000
        }
        #endregion
    }
}

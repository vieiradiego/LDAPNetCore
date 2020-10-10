using AgentNetCore.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Net;

namespace AgentNetCore.Context
{


    public class LDAPContext
    {
        private LdapConnection _ldapConnection;
        private string _ldapServer;
        private NetworkCredential _credential;
        private DirectoryEntry _dirEntry;


        public LDAPContext()
        {

            _dirEntry = new DirectoryEntry();
            _dirEntry.Path = "LDAP://192.168.0.99:389/cn=users,dc=marveldomain,dc=local";
            _dirEntry.AuthenticationType = AuthenticationTypes.Secure;
            _dirEntry.Username = "administrator";
            _dirEntry.Password = "IronMan2000.";
            _credential = new NetworkCredential(_dirEntry.Username, "Pitoca@1988.", "marveldomain.local");
        }

        public bool ResetPassByEmail(string email)
        {

            try
            {
                DirectoryEntry dirEntry = new DirectoryEntry("LDAP://192.168.0.99:389/cn=users,dc=marveldomain,dc=local", "administrator", "IronMan2000.");
                DirectorySearcher ds = new DirectorySearcher(dirEntry);
                SearchResult sr = ds.FindOne();
                if (sr != null)
                {
                    dirEntry.Invoke("ChangePassword", new object[] { "Janete1988." });
                    dirEntry.CommitChanges();
                    dirEntry.Close();
                    Console.WriteLine("\r\nPassword for " + email + " changed successfully");
                }
            }
            catch (Exception e)
            {

                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }
            return true;
        }
        public User Create(User user)
        {
            try
            {

                //new DirectoryEntry("LDAP://192.168.0.99:389/cn=users,dc=marveldomain,dc=local", "administrator", "IronMan2000.");
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
                
                int userAccountControlValue = (int)newUser.Properties["userAccountControl"].Value;
                UserAccountControl userAccountControl = (UserAccountControl)userAccountControlValue;
                newUser.Properties["userAccountControl"].Value = userAccountControl & UserAccountControl.NORMAL_ACCOUNT & UserAccountControl.DONT_EXPIRE_PASSWD;
                newUser.CommitChanges();
                _dirEntry.Close();
                newUser.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }
            return user;
        }
        public void Update(User user)
        {
            Open();
            //Implementar
            Close();
        }
        public User FindByName(string name)
        {
            try
            {
                User user = new User();
                DirectoryEntry dirEntry = new DirectoryEntry();
                dirEntry.Path = "LDAP://192.168.0.99:389/cn=users,dc=marveldomain,dc=local";
                dirEntry.AuthenticationType = AuthenticationTypes.Secure;
                dirEntry.Username = "administrator";
                dirEntry.Password = "IronMan2000.";
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = "(cn=" + name + ")";

                user = GetResult(search.FindOne());
                return user;
            }

            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }
            return null;
        }
        public void Delete(User user)
        {

        }
        public void Open()
        {
            try
            {
                _ldapServer = "192.168.0.99";
                // Create the new LDAP connection
                _ldapConnection = new LdapConnection(_ldapServer);
                _ldapConnection.Credential = _credential;
                _ldapConnection.SessionOptions.DomainName = "MarvelDomain.local";
                _ldapConnection.SessionOptions.AutoReconnect = true;
                _ldapConnection.SessionOptions.HostName = "MarvelServer1";
                _ldapConnection.SessionOptions.PingLimit = 9999;
                _ldapConnection.Bind();
                Console.WriteLine("LdapConnection is created successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }
        }
        public void Close()
        {
            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }

        }

        private User GetResult(SearchResult result)
        {
            if (result != null)
            {
                ResultPropertyCollection fields = result.Properties;
                foreach (String ldapField in fields.PropertyNames)
                {
                    foreach (Object myCollection in fields[ldapField])
                    {

                        Console.WriteLine(String.Format("{0,-20} : {1}", ldapField, myCollection.ToString()));
                    }
                }
            }

            else
            {
                Console.WriteLine("User not found!");
            }
            return null;
        }
        public Computer Create(Computer computer)
        {
            return null;
        }

        public Group Create(Group group)
        {
            return null;
        }

        public Domain Create(Domain domain)
        {
            return null;
        }

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

    }

}


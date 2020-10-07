using AgentNetCore.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Net;

namespace AgentNetCore.Context
{

    public class LDAPContext
    {
        private LdapConnection ldapConnection;
        private string ldapServer;
        private NetworkCredential credential;
        
        public LDAPContext()
        {

        }

        public User Create(User user)
        {
            string oGUID = string.Empty;
            try
            {
                DirectoryEntry dirEntry =
                new DirectoryEntry("LDAP://192.168.0.99:389/cn=users,dc=marveldomain,dc=local", "administrator", "IronMan2000.");
                DirectoryEntry newUser = dirEntry.Children.Add("CN=" + user.Name, "user");
                newUser.Properties["samAccountName"].Value = user.SamAccountName;
                newUser.CommitChanges();
                oGUID = newUser.Guid.ToString();
                int val = (int)newUser.Properties["userAccountControl"].Value;
                newUser.Properties["userAccountControl"].Value = val & ~0x2;
                newUser.CommitChanges();
                dirEntry.Close();
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
                search.Filter = "(cn=" + ""+ name +"" + ")";
                SearchResult result = search.FindOne();
                
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
                ldapServer = "192.168.0.99";
                credential = new NetworkCredential("administrator", "Pitoca@1988.", "marveldomain.local");
                // Create the new LDAP connection
                ldapConnection = new LdapConnection(ldapServer);
                ldapConnection.Credential = credential;
                ldapConnection.SessionOptions.DomainName = "MarvelDomain.local";
                ldapConnection.SessionOptions.AutoReconnect = true;
                ldapConnection.SessionOptions.HostName = "MarvelServer1";
                ldapConnection.SessionOptions.PingLimit = 9999;
                ldapConnection.Bind();


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
    }
}


using AgentNetCore.Model;
using Microsoft.EntityFrameworkCore;
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
        private string targetOU;

        public LDAPContext()
        {

        }

        public User Create(User user)
        {
            Open();
            //Implementar
            Close();
            return user;
        }
        public void Update(User user)
        {
            Open();
            //Implementar
            Close();
        }
        public List<User> FindAll()
        {
            try
            {
                // create LDAP connection object  

                Open();

                // create search object which operates on LDAP connection object  
                // and set search object to only find the user specified  

                DirectoryEntry dirEntry = new DirectoryEntry();
                dirEntry.Path = "LDAP://192.168.0.99:389/cn=users,dc=marveldomain,dc=local";
                dirEntry.AuthenticationType = AuthenticationTypes.Secure;
                dirEntry.Username = "administrator";
                dirEntry.Password = "Pitoca@1988.";

                DirectorySearcher search = new DirectorySearcher(dirEntry);
                search.Filter = "(cn=" + "Diego Vieira" + ")";
                // create results objects from search object  

                SearchResult result = search.FindOne();

                if (result != null)
                {
                    // user exists, cycle through LDAP fields (cn, telephonenumber etc.)  

                    ResultPropertyCollection fields = result.Properties;

                    foreach (String ldapField in fields.PropertyNames)
                    {
                        // cycle through objects in each field e.g. group membership  
                        // (for many fields there will only be one object such as name)  

                        foreach (Object myCollection in fields[ldapField])
                            Console.WriteLine(String.Format("{0,-20} : {1}", ldapField, myCollection.ToString()));
                    }
                }

                else
                {
                    // user does not exist  
                    Console.WriteLine("User not found!");
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }


            return null;
        }
        public void Delete(User user)
        {
            Open();
            //Implementar
            Close();
        }

        public void Open()
        {
            try
            {
                ldapServer = "192.168.0.99";
                credential = new NetworkCredential("administrator", "Pitoca@1988.", "marveldomain.local");
                targetOU = "cn=users,dc=marveldomain,dc=local";
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


using System;
using System.Net;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Security.Permissions;

namespace AgentNetCore.Context
{
    [DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]

    public class LDAPConnect
    {
        private LdapConnection ldapConnection;
        private string ldapServer;
        private NetworkCredential credential;
        private string targetOU;
        
        public LDAPConnect()
        {

        }
        public void Open()
        {
            try
            {
                ldapServer = "192.168.0.99";
                credential = new NetworkCredential("administrator", "Pitoca@1988.", "marveldomain.local");
                targetOU = "OU=users,dc=marveldomain,dc=local";
                // Create the new LDAP connection
                ldapConnection = new LdapConnection(ldapServer);
                ldapConnection.Credential = credential;
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


using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Xml;

namespace AgentNetCore.Context
{
    public class LDAPRouter
    {
        private MySQLContext _context;
        List<string> _servers;
        LdapConnection _ldapConnection;
        private string _user;
        private string _pass;
        private string _domain;
        public LDAPRouter(MySQLContext context)
        {
            _context = context;
            _user = "administrator";
            _pass = "IronMan2000.";
            _domain = "marveldomain.local";
            _servers = new List<string>();
            GetServer();
        }

        private List <string> GetServer()
        {
            string[] array = { "192.168.0.99", "192.168.0.100" };
            List<String> servers = new List<string>(array);
            for (int i = 0; i < servers.Count; i++)
            {
                foreach (var server in servers)
                {
                    if (Test(server, "389"))
                    {
                        _servers.Add(server);
                    }
                }
            }
            
            return _servers;
        }

        private List<string> SetServer(List<string> servers)
        {
            return null;
        }
        private Boolean Test (string _ldapServer, string _portNumber)
        {
            try
            {
                _ldapConnection = new LdapConnection(new LdapDirectoryIdentifier(_ldapServer, Int16.Parse(_portNumber)), new NetworkCredential(_user, _pass, _domain), AuthType.Basic);
                _ldapConnection.Bind();
                Console.WriteLine("LdapConnection is created successfully.");
                
                return true;
            }
            catch (Exception e)
            {

                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return false;
            }
        }
    }
}

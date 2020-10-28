using AgentNetCore.Model;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;

namespace AgentNetCore.Context
{
    public class LDAPConnect
    {

        private MySQLContext _mysqlContext;
        private PrincipalContext _principalContext;
        private LdapConnection _ldapConnection;
        private List <Server> _ldapServer;
        private Credential _credential;
        private string _container;
        private string _path;
        private string _domain;
        

        public LDAPConnect(string domain, ObjectCategory objectCategory)
        {
            _domain = domain;
            _mysqlContext = new MySQLContext();
            _credential = new Credential(GetCredentials(domain));
            _ldapServer = new List<Server>(GetServers(domain));
            _container = Container(objectCategory, _domain);
            _path = SetPath(GetServer(_ldapServer));
            _principalContext = new PrincipalContext(ContextType.Domain, _domain, _container, _credential.User, _credential.Pass);
        }

        private string Container(ObjectCategory objectCategory, string domain)
        {
            string cont = "";
            string[] d = domain.Split(".");
            if (objectCategory == ObjectCategory.user)
            {
                cont = "cn=" + objectCategory;
                for (int i = 0; i < d.Length; i++)
                {

                    cont = cont + ",dc=" + d[i];
                }
                
            }
            if (objectCategory == ObjectCategory.computer) 
            {
                
            }
            if (objectCategory == ObjectCategory.group) 
            {
                for (int i = 0; i < d.Length; i++)
                {
                    _container = _container + "dc=" + d[i];
                    if (d.Length != (i + 1))
                    {
                        _container = _container + ",";
                    }
                }
                
            }
            if (objectCategory == ObjectCategory.organizationalUnit) 
            {
                
            }
            return cont;
        }
        private string SetPath(Server server)
        {
           return "LDAP://" + server.Address + ":" + server.Port+ "/" + _container;
        }
        public Server GetServer(List<Server> servers)
        {
            try
            {
                foreach (var server in servers)
                {
                    _ldapConnection = new LdapConnection(new LdapDirectoryIdentifier(server.Address, Int16.Parse(server.Port)), new NetworkCredential(_credential.User, _credential.Pass, _domain), AuthType.Basic);
                    _ldapConnection.Bind();
                    Console.WriteLine("LdapConnection is created successfully to server " + server.Address + ":" + server.Port);
                    return server;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                
            }
            return new Server();
        }

        private List<Server> GetServers(string domain)
        {
            return (List<Server>)_mysqlContext.Servers.ToList().Where(p =>p.Domain.Equals(domain));
        }

        private Credential GetCredentials(string domain)
        {
            return _mysqlContext.Credentials.SingleOrDefault(p => p.Domain.Equals(domain));
        }

        public enum ObjectCategory
        {// Tipos de Objetos do LDAP
            user, 
            group, 
            computer, 
            organizationalUnit,
            domain,
            person
        }

        public static bool Exists(string objectPath)
        {
            bool found = false;
            if (DirectoryEntry.Exists("LDAP://" + objectPath))
            {
                found = true;
            }
            return found;
        }
        
        public LdapConnection Connection { get => _ldapConnection; }
        public string Path { get => _path; }
        public string User { get => _credential.User; }
        public string Pass { get => _credential.Pass; }
        public string Domain { get => _domain; }
        public PrincipalContext Context { get => _principalContext; }
    }
}


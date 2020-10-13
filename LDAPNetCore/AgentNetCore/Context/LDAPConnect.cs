using System;
using System.Net;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Security.Permissions;
using System.IO;
using System.DirectoryServices.AccountManagement;

namespace AgentNetCore.Context
{
    public class LDAPConnect
    {

        PrincipalContext _context;

        private LdapConnection _ldapConnection;
        private string _ldapServer;
        private string _container;
        private string _path;
        private string _user;
        private string _pass;
        private string _domain;
        private int _portNumber = 389;

        public LDAPConnect()
        {
            _container = "cn=users,dc=marveldomain,dc=local";
            _user = "administrator";
            _pass = "IronMan2000.";
            _domain = "marveldomain.local";
            _ldapServer = "192.168.0.99";
            _path = "LDAP://192.168.0.99:389/cn=users,dc=marveldomain,dc=local";
            _context = new PrincipalContext(ContextType.Domain, _domain, _container, _user, _pass);
            _ldapConnection = new LdapConnection(new LdapDirectoryIdentifier(_ldapServer, _portNumber), new NetworkCredential(_user, _pass, _domain), AuthType.Basic);
        }
        public Boolean Test()
        {
            try
            {
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

        public void Open()
        {
            try
            {

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

        public LdapConnection Connection { get => _ldapConnection; }
        public string Server { get => _ldapServer; }
        public string Path { get => _path; }
        public string User { get => _user; }
        public string Pass { get => _pass; }
        public string Domain { get => _domain; }
        public int PortNumber { get => _portNumber; }
        public PrincipalContext Context { get => _context; }
    }
}


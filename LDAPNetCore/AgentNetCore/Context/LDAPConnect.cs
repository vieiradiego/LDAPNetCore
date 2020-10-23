using System;
using System.Net;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Security.Permissions;
using System.IO;
using System.DirectoryServices.AccountManagement;
using System.ComponentModel;

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
        private string _portNumber = "389";
        private string _portNumberSec = "636";
        private string _commonName;
        private bool _sec;

        public LDAPConnect()
        {
            _user = "administrator";
            _pass = "IronMan2000.";
            _domain = "marveldomain.local";
            _ldapServer = "192.168.0.99";
            _container = "cn=users,dc=marveldomain,dc=local";
            _path = "LDAP://192.168.0.99:" + _portNumber + "/cn=users,dc=marveldomain,dc=local";
            _context = new PrincipalContext(ContextType.Domain, _domain, _container, _user, _pass);
            _ldapConnection = new LdapConnection(new LdapDirectoryIdentifier(_ldapServer, Int16.Parse(_portNumber)), new NetworkCredential(_user, _pass, _domain), AuthType.Basic);
            Test();
        }
        public LDAPConnect(string commonName, string domain, string ldapServer, bool sec)
        {
            _user = "administrator";
            _pass = "IronMan2000.";
            _domain = domain;
            _ldapServer = ldapServer;
            _commonName = commonName;
            _sec = sec;
            Container();
            Sec();
            _context = new PrincipalContext(ContextType.Domain, _domain, _container, _user, _pass);
            _ldapConnection = new LdapConnection(new LdapDirectoryIdentifier(_ldapServer, Int16.Parse(_portNumber)), new NetworkCredential(_user, _pass, _domain), AuthType.Basic);
            Test();
        }

        public LDAPConnect(string domain, string ldapServer, bool sec)
        {
            _user = "administrator";
            _pass = "IronMan2000.";
            _domain = domain;
            _ldapServer = ldapServer;
            _sec = sec;
            Container();
            Sec();
            _context = new PrincipalContext(ContextType.Domain, _domain, _container, _user, _pass);
            _ldapConnection = new LdapConnection(new LdapDirectoryIdentifier(_ldapServer, Int16.Parse(_portNumber)), new NetworkCredential(_user, _pass, _domain), AuthType.Basic);
            Test();
        }

        private void Container()
        {
            string[] d = _domain.Split(".");
            if (_commonName != null)
            {
                _container = "cn=" + _commonName;
                for (int i = 0; i < d.Length; i++)
                {

                    _container = _container + ",dc=" + d[i];
                }
            }
            else
            {
                for (int i = 0; i < d.Length; i++)
                {
                    _container = _container + "dc=" + d[i];
                    if (d.Length != (i+1))
                    {
                        _container = _container + ",";
                    }
                }
            }
        }
        private void Sec()
        {
            if (_sec)
            {
                _path = "LDAP://" + _ldapServer + ":" + _portNumberSec + "/" + _container;
            }
            else
            {
                _path = "LDAP://" + _ldapServer + ":" + _portNumber + "/" + _container;
            }
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
        public enum ObjectClass
        {
            user, group, computer
        }
        public enum ReturnType
        {
            distinguishedName, ObjectGUID
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
        public string Server { get => _ldapServer; }
        public string Path { get => _path; }
        public string User { get => _user; }
        public string Pass { get => _pass; }
        public string Domain { get => _domain; }
        public string PortNumber { get => _portNumber; }
        public PrincipalContext Context { get => _context; }
    }
}


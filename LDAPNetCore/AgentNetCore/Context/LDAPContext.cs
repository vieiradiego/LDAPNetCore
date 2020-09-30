using System.DirectoryServices.Protocols;
using System;
using System.Net;
using System.Security;

namespace AgentNetCore.Context
{

    public class LDAPContext
    {
        private const int _port = 389;
        private const int _portSecurity = 636;
        private string _targetServer;
        private string _dn;
        private LdapDirectoryIdentifier _identifier;
        private NetworkCredential _creds;
        private string _secureString;
        private string _userName;
        public LDAPContext(string targetServer, string dn)
        {
            _targetServer = targetServer; //"ldap.forumsys.com"
            _dn = dn;                     // DN: ou=mathematicians,dc=example,dc=com
            _identifier = new LdapDirectoryIdentifier(_targetServer, _port);
            _userName = "cn=read-only-admin,dc=example,dc=com";
            _secureString = "password";
            _creds = new NetworkCredential(_userName, _secureString); //Credenciais do Usuário e Senha
            LdapConnection _connection = new LdapConnection(_identifier, _creds)
            {
                AuthType = AuthType.Basic,
                           SessionOptions = { ProtocolVersion = 3, SecureSocketLayer = true}
            };
            
            //SearchRequest searchRequest = new SearchRequest("dn=example,dn=com", "(sn=Smith)", SearchScope.Subtree, null);
            //LdapStyleUriParser ldapStyleUriParser = new LdapStyleUriParser();
            //LdapSessionOptions ldapSessionOptions;
            //LdapException ldapEx = new LdapException();
            // string dvv = ldapSessionOptions.HostName;
        }
    }
}


using AgentNetCore.Application;
using AgentNetCore.Model;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace AgentNetCore.Context
{
    public class CredentialRepository
    {
        private string _Domain;
        private string _User;
        private string _Pass;

        private readonly MySQLContext _mySQLContext;
        public CredentialRepository(MySQLContext mySQLContext)
        {
            _mySQLContext = mySQLContext;
        }
        private Credential GetCredentials(string domain)
        {
            return _mySQLContext.Credentials.FirstOrDefault(p => p.Domain.Equals(domain));
        }
        private void SetCredential(string domain)
        {
            Credential c = new Credential();
            c = GetCredentials(domain);
            _User = c.User;
            _Pass = c.Pass;
        }

        public string Domain
        {
            get
            {
                return _Domain;
            }
            set
            {// Ação: quando setar o domínio deve fazer:
             // - Procurar no banco na tabela Credencial
             // - Pegar o primeiro registro e popular os campos: user, Pass, Path
                SetCredential(value);
                _Domain = value;
            }
        }
       
        public string User
        {
            get
            {
                return _User;
            }
        }
        public string Pass
        {
            get
            {
                return _Pass;
            }
        }
    }
}



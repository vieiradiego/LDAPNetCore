using AgentNetCore.Model;
using AgentNetCore.Repository;
using AgentNetCore.Security.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;


namespace AgentNetCore.Context
{
    public class CredentialRepository : ICredentialRepository
    {
        private readonly MySQLContext _mySQLContext;
        private string _Path;
        private string _Domain;
        private string _DN;
        private string _User;
        private string _Pass;
        public string Domain { get { return _Domain; } }
        public string Path { get { return _Path; } }
        public string User { get { return _User; } }
        public string Pass { get { return _Pass; } }

        public CredentialRepository(MySQLContext mySQLContext)
        {
            _mySQLContext = mySQLContext;
        }

        public Credential Create(Credential credential)
        {
            // Criptografar
            credential.Pass = Encrypt(credential.Pass);
            credential.User = Encrypt(credential.User);
            credential.Domain = Encrypt(credential.Domain);
            _mySQLContext.Add(credential);
            _mySQLContext.SaveChanges();
            Credential teste = _mySQLContext.Credentials.FirstOrDefault(c => ((c.User == credential.User) && (c.Pass == credential.Pass)));
            return teste;
        }
        private bool Exist(long? id)
        {
            return _mySQLContext.Credentials.Any(p => p.Id.Equals(id));
        }
        public Credential Update(Credential credential)
        {
            // Criptografar
            if (!Exist(credential.Id)) return new Credential();
            var result = _mySQLContext.Credentials.SingleOrDefault(p => p.Id.Equals(credential.Id));
            try
            {
                credential.Pass = Encrypt(credential.Pass);
                _mySQLContext.Entry(result).CurrentValues.SetValues(credential);
                _mySQLContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
            return _mySQLContext.Credentials.FirstOrDefault(c => ((c.User == credential.User) && (c.Pass == credential.Pass)));
        }
        public string DN
        {
            get
            {
                return _DN;
            }
            set
            {// Receber SOMENTE DN ("OU=Users,OU=UnitedStates,OU=Marvel,OU=Marvel Company,DC=MarvelDomain,DC=local",)
             // Converter a DN em dominio
             // Converter a DN para Path LDAP
             // Procurar no banco as credenciais disponíveis para o dominio
             // Procurar os servidores disponíveis
             // Popular os campos: user, pass, dn, path LDAP
                SetDomainByDN(value);
                SetDN(value);
                SetPathByDN(value);
                SetCredential();
            }
        }
        private void SetDN(string value)
        {
            _DN = value;
        }
        private void SetDomainByDN(string value)
        {
            _Domain = DnToDomain(value);
        }
        private void SetPathByDN(string value)
        {
            _Path = DnToPath(value);
        }
        private string DnToDomain(string dn)
        {
            dn = dn.ToLower();
            string[] d = dn.Split(",");
            string domain = "";
            for (int i = 0; i < d.Length; i++)
            {
                if (d[i].Contains("dc="))
                {
                    domain = domain + d[i];
                    domain = domain.Replace("dc=", "");
                    domain = domain.ToLower();
                    if (d.Length != (i + 1))
                    {
                        domain = domain + ".";
                    }
                }
            }
            return domain;
        }
        private string DnToPath(string dn)
        {
            try
            {
                ServerRepository sr = new ServerRepository(_mySQLContext);
                List<Server> servers = sr.GetServers(_Domain);
                return "LDAP://" + servers[0].Address + ":" + servers[0].Port + "/" + dn;
            }
            catch (Exception e )
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        private string PathToDn(string dn)
        {
            try
            {
                ServerRepository sr = new ServerRepository(_mySQLContext);
                List<Server> servers = sr.GetServers(_Domain);
                return "LDAP://" + servers[0].Address + ":" + servers[0].Port + "/" + dn;
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        private void SetCredential()
        {
            if ((_User == null) && (_Pass == null))
            {
                try
                {
                    Credential c = new Credential();
                    c = GetCredentials(Encrypt(_Domain));
                    _User = Decrypt(c.User);
                    _Pass = Decrypt(c.Pass);
                }
                catch (Exception e)
                {
                    Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                }
            }
        }
        private Credential Find(Credential credential)
        {
            return _mySQLContext.Credentials.FirstOrDefault(p => p.User.Equals(credential.User));
        }
        private Credential GetCredentials(string domain)
        {
            // Descriptografar
            // Entregar as credenciais corretas

            return _mySQLContext.Credentials.FirstOrDefault(p => p.Domain.Equals(domain));
        }

        public string Encrypt(string textToEncrypt)
        {
            try
            {
                string ToReturn = "";
                string publickey = "publmarv";
                string secretkey = "privmarv";
                byte[] secretkeyByte = { };
                secretkeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    ToReturn = Convert.ToBase64String(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }

        public string Decrypt(string textToDecrypt)
        {
            try
            {
                
                string ToReturn = "";
                string publickey = "publmarv";
                string privatekey = "privmarv";
                byte[] privatekeyByte = { };
                privatekeyByte = System.Text.Encoding.UTF8.GetBytes(privatekey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
                inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    ToReturn = encoding.GetString(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
    }
}



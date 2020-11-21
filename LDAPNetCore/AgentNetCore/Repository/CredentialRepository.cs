using AgentNetCore.Application;
using AgentNetCore.Model;
using AgentNetCore.Repository;
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
    public class CredentialRepository : ICredentialRepository
    {
        private readonly MySQLContext _mySQLContext;
        private string _Path;
        private string _Domain;
        private string _DN;
        private string _CN;
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
            ServerRepository sr = new ServerRepository(_mySQLContext);
            List<Server> servers = sr.GetServers(_Domain);
            return "LDAP://" + servers[0].Address + ":" + servers[0].Port + "/" + dn;
        }
        private void SetCredential()
        {
            if ((_User == null) && (_Pass == null))
            {
                try
                {
                    Credential c = new Credential();
                    c = GetCredentials(_Domain);
                    _User = c.User;
                    _Pass = c.Pass;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        private Credential GetCredentials(string domain)
        {
            return _mySQLContext.Credentials.FirstOrDefault(p => p.Domain.Equals(domain));
        }

        //public string GetPathByServer(string domain)
        //{// INPUT  marveldomain.local
        // // OUTPUT Path
        //    List<Server> servers = GetServers(domain);
        //    foreach (var server in servers)
        //    {
        //        string path = SetPath(server);
        //        if (Exists(path))
        //        {
        //            return path;
        //        }
        //    }
        //    return null;
        //}

        
        //public string GetPathByDN(string distinguishedName)
        //{// Input  (distinguishedName) OU=Users,OU=UnitedStates,OU=Marvel,OU=Marvel Company,DC=MarvelDomain,DC=local
        // // Output (LDAP Path) LDAP://SERVER:PORT/OU=Users,OU=UnitedStates,OU=Marvel,OU=Marvel Company,DC=MarvelDomain,DC=local

        //    
        //}
        //public string ConvertToDomain(string value)
        //{// Input  (distinguishedName, Path, email) OU=Users,OU=UnitedStates,OU=Marvel,OU=Marvel Company,DC=MarvelDomain,DC=local
        // // Output (domain) marveldomain.local
        //    string cont = "";
        //    value = value.ToLower();
        //    if (value.Contains("@"))
        //    {//Email
        //        string[] d = value.Split("@");
        //        return d[1];
        //    }
        //    if (value.Contains("ldap://"))
        //    {// Path
        //        string[] d = value.Split("/");
        //        for (int i = 0; i < d.Length; i++)
        //        {
        //            if ((d[i].Contains("dc=")) && (d[i].Contains(",")))
        //            {
        //                string[] g = d[i].Split(",");
        //                for (int e = 0; e < g.Length; e++)
        //                {
        //                    if (g[e].Contains("dc="))
        //                    {
        //                        cont = cont + g[e];
        //                        cont = cont.Replace("dc=", "");
        //                        cont = cont.ToLower();
        //                        if (g.Length != (e + 1))
        //                        {
        //                            cont = cont + ".";
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        return cont;
        //    }
        //    if (value.Contains("."))
        //    {
        //        return value;
        //    }
        //    else
        //    {//DistinguishedName
        //        string[] d = value.Split(",");
        //        for (int i = 0; i < d.Length; i++)
        //        {
        //            if (d[i].Contains("dc="))
        //            {
        //                cont = cont + d[i];
        //                cont = cont.Replace("dc=", "");
        //                cont = cont.ToLower();
        //                if (d.Length != (i + 1))
        //                {
        //                    cont = cont + ".";
        //                }
        //            }
        //        }
        //        return cont;
        //    }
        //}
        //public string RemoveCN(string value)
        //{// INPUT (Path ) ldap://192.168.0.99:389/cn=user,ou=users,ou=unitedstates,ou=marvel,ou=marvel company,dc=marveldomain,dc=local
        // // OUTPUT (Path no <cn=> tag) ldap://192.168.0.99:389/ou=users,ou=unitedstates,ou=marvel,ou=marvel company,dc=marveldomain,dc=local
        //    string path = "";
        //    value = value.ToLower();
        //    if (value.Contains("ldap://"))
        //    {
        //        string[] itens1 = value.Split(",");
        //        for (int i = 0; i < itens1.Length; i++)
        //        {
        //            if (itens1[i].Contains("ldap:"))
        //            {//Splitar na Barra

        //                string[] itens2 = itens1[i].Split("/");
        //                for (int j = 0; j < itens2.Length; j++)
        //                {
        //                    if (itens2[j].Contains("ldap:"))
        //                    {
        //                        path = itens2[j].ToUpper() + "//";
        //                        continue;
        //                    }
        //                    if (itens2[j].Contains(":"))
        //                    {
        //                        path = path + itens2[j] + "/";
        //                        continue;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                path = path + itens1[i];
        //                if (itens1.Length != (i + 1))
        //                {
        //                    path = path + ",";
        //                }
        //            }
        //        }
        //    }
        //    return path;
        //}
    }
}



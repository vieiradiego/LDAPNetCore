using AgentNetCore.Context;
using AgentNetCore.Model;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace AgentNetCore.Repository
{
    public class ServerRepository
    {
        private readonly MySQLContext _mySQLContext;
        public ServerRepository(MySQLContext mySQLContext)
        {
            _mySQLContext = mySQLContext;
        }

        public string GetPathByServer(string domain)
        {// INPUT  marveldomain.local
         // OUTPUT Path
            List<Server> servers = GetServers(domain);
            foreach (var server in servers)
            {
                string path = SetPath(server);
                if (Exists(path))
                {
                    return path;
                }
            }
            return null;
        }
        private List<Server> GetServers(string domain)
        {
            return _mySQLContext.Servers.Where(p => p.Domain.Equals(domain)).ToList();
        }
        private string SetPath(Server server)
        {
            return "LDAP://" + server.Address + ":" + server.Port + "/" + server.Container;
        }
        public string GetPathByDN(string distinguishedName)
        {// Input  (distinguishedName) OU=Users,OU=UnitedStates,OU=Marvel,OU=Marvel Company,DC=MarvelDomain,DC=local
         // Output (LDAP Path) LDAP://SERVER:PORT/OU=Users,OU=UnitedStates,OU=Marvel,OU=Marvel Company,DC=MarvelDomain,DC=local

            List<Server> servers = GetServers(ConvertToDomain(distinguishedName));
            foreach (var server in servers)
            {
                string path = "LDAP://" + server.Address + ":" + server.Port + "/" + distinguishedName;
                if (Exists(path))
                {
                    return path;
                }
            }
            return null;
        }
        public string ConvertToDomain(string value)
        {// Input  (distinguishedName, Path, email) OU=Users,OU=UnitedStates,OU=Marvel,OU=Marvel Company,DC=MarvelDomain,DC=local
         // Output (domain) marveldomain.local
            string cont = "";
            value = value.ToLower();
            if (value.Contains("@"))
            {//Email
                string[] d = value.Split("@");
                return d[1];
            }
            if (value.Contains("ldap://"))
            {// Path
                string[] d = value.Split("/");
                for (int i = 0; i < d.Length; i++)
                {
                    if ((d[i].Contains("dc=")) && (d[i].Contains(",")))
                    {
                        string[] g = d[i].Split(",");
                        for (int e = 0; e < g.Length; e++)
                        {
                            if (g[e].Contains("dc="))
                            {
                                cont = cont + g[e];
                                cont = cont.Replace("dc=", "");
                                cont = cont.ToLower();
                                if (g.Length != (e + 1))
                                {
                                    cont = cont + ".";
                                }
                            }
                        }
                    }
                }
                return cont;
            }
            else
            {//DistinguishedName
                string[] d = value.Split(",");
                for (int i = 0; i < d.Length; i++)
                {
                    if (d[i].Contains("dc="))
                    {
                        cont = cont + d[i];
                        cont = cont.Replace("dc=", "");
                        cont = cont.ToLower();
                        if (d.Length != (i + 1))
                        {
                            cont = cont + ".";
                        }
                    }
                }
                return cont;
            }
        }
        public string RemoveCN(string value)
        {// INPUT (Path ) ldap://192.168.0.99:389/cn=user,ou=users,ou=unitedstates,ou=marvel,ou=marvel company,dc=marveldomain,dc=local
         // OUTPUT (Path no <cn=> tag) ldap://192.168.0.99:389/ou=users,ou=unitedstates,ou=marvel,ou=marvel company,dc=marveldomain,dc=local
            string path = "";
            value = value.ToLower();
            if (value.Contains("ldap://"))
            {
                string[] itens1 = value.Split(",");
                for (int i = 0; i < itens1.Length; i++)
                {
                    if (itens1[i].Contains("ldap:"))
                    {//Splitar na Barra

                        string[] itens2 = itens1[i].Split("/");
                        for (int j = 0; j < itens2.Length; j++)
                        {
                            if (itens2[j].Contains("ldap:"))
                            {
                                path = itens2[j].ToUpper() + "//";
                                continue;
                            }
                            if (itens2[j].Contains(":"))
                            {
                                path = path + itens2[j] + "/";
                                continue;
                            }
                        }
                    }
                    else
                    {
                        path = path + itens1[i];
                        if (itens1.Length != (i + 1))
                        {
                            path = path + ",";
                        }
                    }
                }
            }
            return path;
        }
        public static bool Exists(string path)
        {
            try
            {
                bool found = false;
                if (DirectoryEntry.Exists(path))
                {
                    found = true;
                }
                return found;
            }
            catch (Exception e)
            {
                if (e.Message.Equals("The user name or password is incorrect."))
                {
                    return true;
                }
                return false;
            }
        }
    }
}

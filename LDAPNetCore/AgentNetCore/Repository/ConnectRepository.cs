using AgentNetCore.Application;
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
    public class ConnectRepository
    {
        public ConnectRepository()
        {
            
        }

        private string Container(ObjectApplication.Category objectCategory, string domain)
        {
            string cont = "";
            string[] d = domain.Split(".");
            if (objectCategory == ObjectApplication.Category.user)
            {
                cont = "cn=" + objectCategory;
                for (int i = 0; i < d.Length; i++)
                {

                    cont = cont + ",dc=" + d[i];
                }

            }
            if (objectCategory == ObjectApplication.Category.computer)
            {

            }
            if (objectCategory == ObjectApplication.Category.group)
            {
                for (int i = 0; i < d.Length; i++)
                {
                    cont = cont + "dc=" + d[i];
                    if (d.Length != (i + 1))
                    {
                        cont = cont + ",";
                    }
                }

            }
            if (objectCategory == ObjectApplication.Category.organizationalUnit)
            {

            }
            return cont;
        }
        private string SetPath(Server server)
        {
            return "LDAP://" + server.Address + ":" + server.Port + "/" + server.Container;
        }
        private Server GetServer(string domain, List<Server> servers)
        {
            // Get All server in table to MySQL Server
            try
            {
                foreach (var server in servers)
                {

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
            MySQLContext mysqlContext = new MySQLContext();
            return mysqlContext.Servers.Where(p => p.Domain.Equals(domain)).ToList();
        }

        private Credential GetCredentials(string domain)
        {
            MySQLContext mysqlContext = new MySQLContext();
            return mysqlContext.Credentials.FirstOrDefault(p => p.Domain.Equals(domain));
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
    }
}


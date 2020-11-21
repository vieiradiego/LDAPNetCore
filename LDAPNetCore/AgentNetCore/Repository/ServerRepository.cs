using AgentNetCore.Context;
using AgentNetCore.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
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
        private List<Server> FindServers(string domain)
        {
            return _mySQLContext.Servers.Where(p => p.Domain.Equals(domain)).ToList();
        }
        public List<Server> GetServers(string domain)
        {
            List<Server> serversVerify = new List<Server>();
            List<Server> servers = FindServers(domain);
            try
            {
                for (int i = 0; i < servers.Count; i++)
                {

                    if (IsServerUp(servers[i].Address, Int32.Parse(servers[i].Port), 3))
                    {
                        serversVerify.Add(servers[i]);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }

            return serversVerify;
        }

        public static bool IsServerUp(string server, int port, int timeout)
        {
            bool success = false;
            try
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                var client = new TcpClient();
                var result = client.BeginConnect(server, port, null, null);
                success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));
                Console.WriteLine(success ? "Connected to " + server + ":" + port : "Failed to connect  to " + server + ":" + port);
                if (success)
                {
                    client.EndConnect(result);
                }
                stopWatch.Stop();
                Console.WriteLine("Time: (ms) " + stopWatch.ElapsedMilliseconds + "|" + server + ":" + port);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }
            return success;
        }
    }
}


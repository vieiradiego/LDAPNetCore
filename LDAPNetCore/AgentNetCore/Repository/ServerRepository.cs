using AgentNetCore.Context;
using AgentNetCore.Model;
using System;
using System.Collections.Generic;
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
            for (int i = 0; i < servers.Count; i++)
            {
                try
                {
                    if (IsServerUp(servers[i].Address, Int32.Parse(servers[i].Port),3))
                    {
                        serversVerify.Add(servers[i]);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                }
            }
            return serversVerify;
        }
        public static bool IsServerUp(string server, int port, int timeout)
        {
            bool isUp = false;
            try
            {
                using (TcpClient tcp = new TcpClient())
                {
                    IPAddress address;
                    if (IPAddress.TryParse(server, out address))
                    {
                        IAsyncResult ar = tcp.BeginConnect(address, port, null, null);
                        WaitHandle wh = ar.AsyncWaitHandle;

                        try
                        {
                            if (!wh.WaitOne(TimeSpan.FromMilliseconds(timeout), false))
                            {
                                tcp.EndConnect(ar);
                                tcp.Close();
                                throw new SocketException();
                            }

                            isUp = true;
                            tcp.EndConnect(ar);
                        }
                        finally
                        {
                            wh.Close();
                        }
                    }
                }
            }
            catch (SocketException e)
            {
                isUp = false;
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }
            return isUp;
        }

    }
}


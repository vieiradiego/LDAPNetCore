using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgentNetCore.Model
{
    public class Server
    {

        public long? Id { get; set; }
        public string Domain { get; set; }
        public string Address{ get; set; }
        public string Port { get; set; }
    }
}

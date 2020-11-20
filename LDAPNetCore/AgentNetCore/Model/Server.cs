using AgentNetCore.Model.Base;

namespace AgentNetCore.Model
{
    public class Server
    {

        public long? Id { get; set; }
        public string Domain { get; set; }
        public string Address { get; set; }
        public string Port { get; set; }
    }
}

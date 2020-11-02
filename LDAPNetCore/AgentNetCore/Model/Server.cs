using AgentNetCore.Model.Base;

namespace AgentNetCore.Model
{
    public class Server : BaseEntity
    {
        public long? Id { get; set; }
        public string Domain { get; set; }
        public string Address { get; set; }
        public string Port { get; set; }
        public string PathDomain { get; set; }
        public string Container { get; set; }
    }
}

using AgentNetCore.Model.Base;

namespace AgentNetCore.Model
{
    public class Computer : BaseEntity
    {
        public string Description { get; set; }
        public string DnsHostName { get; set; }
        public string Location { get; set; }
    }
}

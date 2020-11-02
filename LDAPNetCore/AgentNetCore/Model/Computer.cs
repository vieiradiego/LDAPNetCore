using AgentNetCore.Model.Base;

namespace AgentNetCore.Model
{
    public class Computer : BaseEntity
    {
        public string SamAccountName { get; set; }
        public string DistinguishedName { get; set; }
        public string PathDomain { get; set; }
    }
}

using AgentNetCore.Data.VO.Base;
using System.Runtime.Serialization;

namespace AgentNetCore.Data.VO
{
    [DataContract]
    public class ComputerVO : BaseVO
    {
        [DataMember(Order = 7)] public string Description { get; set; }
        [DataMember(Order = 8)] public string Location { get; set; }
        [DataMember(Order = 8)] public string DnsHostName { get; set; }
    }
}

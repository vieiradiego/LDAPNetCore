using AgentNetCore.Data.VO.Base;
using System.Runtime.Serialization;

namespace AgentNetCore.Data.VO
{
    [DataContract]
    public class GroupVO : BaseVO
    {
        [DataMember(Order = 7)] public string Description { get; set; }
        [DataMember(Order = 8)] public string EmailAddress { get; set; }
        [DataMember(Order = 9)] public string ObjectSid { get; set; }
        [DataMember(Order = 10)] public string Manager { get; set; }
        
        
    }
}

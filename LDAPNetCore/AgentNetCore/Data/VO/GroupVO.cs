using AgentNetCore.Data.VO.Base;
using System.Runtime.Serialization;

namespace AgentNetCore.Data.VO
{
    [DataContract]
    public class GroupVO : BaseVO
    {
        [DataMember(Order = 7)] public string Description { get; set; }
        [DataMember(Order = 8)] public string EmailAddress { get; set; }
        [DataMember(Order = 10)] public string Manager { get; set; }
        [DataMember(Order = 11)] public bool System { get; set; }
        [DataMember(Order = 12)] public bool Global { get; set; }
        [DataMember(Order = 13)] public bool DomainLocal { get; set; }
        [DataMember(Order = 14)] public bool Universal { get; set; }
        [DataMember(Order = 15)] public bool AppBasic { get; set; }
        [DataMember(Order = 16)] public bool AppQuery { get; set; }
        [DataMember(Order = 17)] public bool Security { get; set; }


    }
}

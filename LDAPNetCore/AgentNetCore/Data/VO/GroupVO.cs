using System.Runtime.Serialization;

namespace AgentNetCore.Data.VO
{
    [DataContract]
    public class GroupVO
    {
        [DataMember(Order = 1)] public string DisplayName { get; set; }
        [DataMember(Order = 2)] public string Name { get; set; }
        [DataMember(Order = 3)] public string Description { get; set; }
        [DataMember(Order = 4)] public string EmailAddress { get; set; }
        [DataMember(Order = 5, IsRequired = true)] public string SamAccountName { get; set; }
        [DataMember(Order = 6)] public string ObjectSid { get; set; }
        [DataMember(Order = 7)] public string Manager { get; set; }
        [DataMember(Order = 8)] public string PathDomain { get; set; }
    }
}

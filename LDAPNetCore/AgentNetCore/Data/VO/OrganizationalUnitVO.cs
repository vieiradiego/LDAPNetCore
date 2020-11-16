using AgentNetCore.Data.VO.Base;
using System.Runtime.Serialization;

namespace AgentNetCore.Data.VO
{
    [DataContract]
    public class OrganizationalUnitVO : BaseVO
    {
        [DataMember(Order = 7)] public string Description { get; set; }
        [DataMember(Order = 8)] public bool ProtectDeletion { get; set; }
        [DataMember(Order = 9)] public string Manager { get; set; }
        [DataMember(Order = 10)] public string City { get; set; }
        [DataMember(Order = 11)] public string State { get; set; }
        [DataMember(Order = 12)] public string PostalCode { get; set; }
        [DataMember(Order = 13)] public string Country { get; set; }
        [DataMember(Order = 14)] public string Email { get; set; }
        [DataMember(Order = 15)] public string ObjectSid { get; set; }
        [DataMember(Order = 16)] public string WhenChanged { get; set; }
        [DataMember(Order = 16)] public string WhenCreated { get; set; }
        [DataMember(Order = 17)] public string Ou { get; set; }
        [DataMember(Order = 18)] public string Street { get; set; }
        [DataMember(Order = 19)] public bool IsCriticalSystemObject { get; set; }
        [DataMember(Order = 20)] public string CommonName { get; set; }
    }
}

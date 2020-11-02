using System.Collections.Generic;
using System.Runtime.Serialization;
using Tapioca.HATEOAS;

namespace AgentNetCore.Data.VO
{
    [DataContract]
    public class ForestVO : ISupportsHyperMedia
    {
        [DataMember(Order = 1)] public string Name { get; set; }
        [DataMember(Order = 2)] public string DisplayName { get; set; }
        [DataMember(Order = 3)] public string Description { get; set; }
        [DataMember(Order = 4)] public bool ProtectDeletion { get; set; }
        [DataMember(Order = 5, IsRequired = true)] public string SamAccountName { get; set; }
        [DataMember(Order = 6)] public string Manager { get; set; }
        [DataMember(Order = 7)] public string PathDomain { get; set; }
        [DataMember(Order = 8)] public string Domain { get; set; }
        [DataMember(Order = 9)] public string City { get; set; }
        [DataMember(Order = 10)] public string State { get; set; }
        [DataMember(Order = 11)] public string PostalCode { get; set; }
        [DataMember(Order = 12)] public string Country { get; set; }
        [DataMember(Order = 13)] public string Email { get; set; }
        [DataMember(Order = 14)] public string ObjectSid { get; set; }
        [DataMember(Order = 15)] public string WhenChanged { get; set; }
        [DataMember(Order = 16)] public string WhenCreated { get; set; }
        [DataMember(Order = 17)] public string Ou { get; set; }
        [DataMember(Order = 18)] public string DistinguishedName { get; set; }
        [DataMember(Order = 19)] public string Street { get; set; }
        [DataMember(Order = 20)] public bool IsCriticalSystemObject { get; set; }
        [DataMember(Order = 21)] public string CommonName { get; set; }
        public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
    }
}

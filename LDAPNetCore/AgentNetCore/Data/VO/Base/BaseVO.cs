using System.Collections.Generic;
using System.Runtime.Serialization;
using Tapioca.HATEOAS;

namespace AgentNetCore.Data.VO.Base
{
    [DataContract]
    public class BaseVO : ISupportsHyperMedia
    {
        [DataMember(Order = 1, Name = "Nome")] public string Name { get; set; }
        [DataMember(Order = 2)] public string DisplayName { get; set; }
        [DataMember(Order = 3)] public string PathDomain { get; set; }
        [DataMember(Order = 4)] public string Domain { get; set; }
        [DataMember(Order = 5, IsRequired = true)] public string? SamAccountName { get; set; }
        [DataMember(Order = 6)] public string? DistinguishedName { get; set; }
        public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
    }
}

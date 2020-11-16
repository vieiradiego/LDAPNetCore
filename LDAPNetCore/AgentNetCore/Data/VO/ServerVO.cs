using System.Collections.Generic;
using System.Runtime.Serialization;
using Tapioca.HATEOAS;

namespace AgentNetCore.Data.VO
{
    [DataContract]
    public class ServerVO : ISupportsHyperMedia
    {
        [DataMember(Order = 1)] public int? Id { get; set; }
        [DataMember(Order = 2)] public string Domain { get; set; }
        [DataMember(Order = 3)] public string Address { get; set; }
        [DataMember(Order = 4)] public string Port { get; set; }
        public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
    }
}

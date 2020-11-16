using System.Collections.Generic;
using System.Runtime.Serialization;
using Tapioca.HATEOAS;

namespace AgentNetCore.Data.VO
{

    /// <summary>
    /// Cliente VO
    /// </summary>
    [DataContract]
    public class ClientVO : ISupportsHyperMedia
    {
        /// <summary>
        /// User Name
        /// </summary>
        [DataMember(Order = 1)] public string UserName { get; set; }
        /// <summary>
        /// Passowrd
        /// </summary>
        [DataMember(Order = 2)] public string Password { get; set; }
        public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
    }
}

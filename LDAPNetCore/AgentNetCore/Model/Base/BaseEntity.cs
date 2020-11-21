using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgentNetCore.Model.Base
{
    public class BaseEntity
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string PathDomain { get; set; }
        public string Domain { get; set; }
        public string? SamAccountName { get; set; }
        public string? DistinguishedName { get; set; }
    }
}

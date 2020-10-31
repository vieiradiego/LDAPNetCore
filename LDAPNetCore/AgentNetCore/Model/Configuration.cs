using AgentNetCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgentNetCore.Model
{
    public class Configuration
    {
        public long? Id { get; set; }
        public string ConfigurationField { get; set; }
        public string Value { get; set; }
    }
}

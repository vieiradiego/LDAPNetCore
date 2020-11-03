using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgentNetCore.Model
{
    public class Client
    {
        public long? Id { get; set; }
        public string SecretClient { get; set; }
        public string SecretKey { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}

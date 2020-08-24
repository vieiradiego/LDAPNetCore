using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Model
{
    public class Group
    {
        public long Id { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public string SamAccountName { get; set; }
        public string ObjectSid { get; set; }
        public string Domain { get; set; }
    }
}

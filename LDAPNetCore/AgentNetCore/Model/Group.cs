using AgentNetCore.Model.Base;

namespace AgentNetCore.Model
{
    public class Group : BaseEntity
    {
        public string Description { get; set; }
        public string EmailAddress { get; set; }
        public string Manager { get; set; }
        public bool System { get; set; }
        public bool Global { get; set; }
        public bool DomainLocal { get; set; }
        public bool Universal { get; set; }
        public bool AppBasic { get; set; }
        public bool AppQuery { get; set; }
        public bool Security { get; set; }
    }
}

using AgentNetCore.Model.Base;

namespace AgentNetCore.Model
{
    public class Group : BaseEntity
    {
        public string Description { get; set; }
        public string EmailAddress { get; set; }
        public string ObjectSid { get; set; }
        public string Manager { get; set; }
    }
}

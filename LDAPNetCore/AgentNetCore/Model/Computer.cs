namespace AgentNetCore.Model
{
    public class Computer
    {
        public long? Id { get; set; }
        public string SamAccountName { get; set; }
        public string DistinguishedName { get; set; }
        public string PathDomain { get; set; }
    }
}

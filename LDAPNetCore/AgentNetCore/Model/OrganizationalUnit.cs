namespace AgentNetCore.Model
{
    public class OrganizationalUnit
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool ProtectDeletion { get; set; }
        public string SamAccountName { get; set; }
        public string Manager { get; set; }
        public string PathDomain { get; set; }
        public string Domain { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}

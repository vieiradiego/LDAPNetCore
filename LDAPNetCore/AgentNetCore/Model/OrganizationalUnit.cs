namespace AgentNetCore.Model
{
    public class OrganizationalUnit
    {
        public string Name { get; set; }
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
        public string Email { get; set; }
        public string ObjectSid { get; set; }
        public string WhenChanged { get; set; }
        public string WhenCreated { get; set; }
        public string Ou { get; set; }
        public string DistinguishedName { get; set; }
        public string Street { get; set; }
        public bool IsCriticalSystemObject { get; set; }
        public string CommonName { get; set; }
    }
}

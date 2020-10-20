using System;

namespace AgentNetCore.Model
{
    public class User
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string LogonName { get; set; }
        public string FirstName { get; set; }
        public string DisplayName { get; set; }
        public string Inicials { get; set; }
        public string OfficePhone { get; set; }
        public string MobilePhone { get; set; }
        public string Description { get; set; }
        public string Organization { get; set; }
        public string Office { get; set; }
        public string EmailAddress { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string StreetAddress { get; set; }
        public string Title { get; set; }
        public string Departament { get; set; }
        public string Company { get; set; }
        public string Manager { get; set; }
        public string EmployeeID { get; set; }
        public bool Enabled { get; set; }
        public string AuthType { get; set; }
        public bool PasswordNotRequired { get; set; }
        public bool PasswordNeverExpires { get; set; }
        public bool CanNotChangePassword { get; set; }
        public bool ChangePasswordAtLogon { get; set; }
        public string AccountPassword { get; set; }
        public DateTime AccountExpirationDate { get; set; }
        public string PathDomain { get; set; }
        public string SamAccountName { get; set; }
        public string DistinguishedName { get; set; }
        public string DirectReports { get; set; }
        public string MemberOf { get; set; }
        public string UserAccountControl { get; set; }
    }
}

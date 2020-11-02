using System;
using System.Runtime.Serialization;

namespace AgentNetCore.Data.VO
{
    [DataContract]
    public class UserVO
    {
        [DataMember(Order = 1, Name = "Name")] public string Name { get; set; }
        [DataMember(Order = 2)] public string Surname { get; set; }
        [DataMember(Order = 3)] public string LogonName { get; set; }
        [DataMember(Order = 4)] public string FirstName { get; set; }
        [DataMember(Order = 5)] public string DisplayName { get; set; }
        [DataMember(Order = 6)] public string Inicials { get; set; }
        [DataMember(Order = 7)] public string OfficePhone { get; set; }
        [DataMember(Order = 8)] public string MobilePhone { get; set; }
        [DataMember(Order = 9)] public string Description { get; set; }
        [DataMember(Order = 10)] public string Office { get; set; }
        [DataMember(Order = 11)] public string EmailAddress { get; set; }
        [DataMember(Order = 12)] public string PostalCode { get; set; }
        [DataMember(Order = 13)] public string Country { get; set; }
        [DataMember(Order = 14)] public string City { get; set; }
        [DataMember(Order = 15)] public string State { get; set; }
        [DataMember(Order = 16)] public string StreetAddress { get; set; }
        [DataMember(Order = 17)] public string Title { get; set; }
        [DataMember(Order = 18)] public string Departament { get; set; }
        [DataMember(Order = 19)] public string Company { get; set; }
        [DataMember(Order = 20)] public string Manager { get; set; }
        [DataMember(Order = 21)] public string EmployeeID { get; set; }
        [DataMember(Order = 22)] public bool Enabled { get; set; }
        [DataMember(Order = 23)] public string AuthType { get; set; }
        [DataMember(Order = 24)] public bool PasswordNotRequired { get; set; }
        [DataMember(Order = 25)] public bool PasswordNeverExpires { get; set; }
        [DataMember(Order = 26)] public bool CanNotChangePassword { get; set; }
        [DataMember(Order = 27)] public bool ChangePasswordAtLogon { get; set; }
        [DataMember(Order = 28)] public string AccountPassword { get; set; }
        [DataMember(Order = 29)] public DateTime AccountExpirationDate { get; set; }
        [DataMember(Order = 30)] public string PathDomain { get; set; }
        [DataMember(Order = 31)] public string Domain { get; set; }
        [DataMember(Order = 32, IsRequired = true)]public string SamAccountName { get; set; }
        [DataMember(Order = 33)] public string DistinguishedName { get; set; }
        [DataMember(Order = 34)] public string DirectReports { get; set; }
        [DataMember(Order = 35)] public string MemberOf { get; set; }
        [DataMember(Order = 36)] public string UserAccountControl { get; set; }
    }
}

using AgentNetCore.Data.VO.Base;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Tapioca.HATEOAS;

namespace AgentNetCore.Data.VO
{
    [DataContract]
    public class UserVO : BaseVO
    {
        [DataMember(Order = 7)] public string FirstName { get; set; }
        [DataMember(Order = 8)] public string Inicials { get; set; }
        [DataMember(Order = 9)] public string Surname { get; set; }
        [DataMember(Order = 10)] public string LogonName { get; set; }
        [DataMember(Order = 11)] public string OfficePhone { get; set; }
        [DataMember(Order = 12)] public string MobilePhone { get; set; }
        [DataMember(Order = 13)] public string Description { get; set; }
        [DataMember(Order = 14)] public string Office { get; set; }
        [DataMember(Order = 15)] public string EmailAddress { get; set; }
        [DataMember(Order = 16)] public string PostalCode { get; set; }
        [DataMember(Order = 17)] public string Country { get; set; }
        [DataMember(Order = 18)] public string City { get; set; }
        [DataMember(Order = 19)] public string State { get; set; }
        [DataMember(Order = 20)] public string StreetAddress { get; set; }
        [DataMember(Order = 21)] public string Title { get; set; }
        [DataMember(Order = 22)] public string Departament { get; set; }
        [DataMember(Order = 23)] public string Company { get; set; }
        [DataMember(Order = 24)] public string Manager { get; set; }
        [DataMember(Order = 25)] public string EmployeeID { get; set; }
        [DataMember(Order = 26)] public bool Enabled { get; set; }
        [DataMember(Order = 27)] public string AuthType { get; set; }
        [DataMember(Order = 28)] public bool PasswordNotRequired { get; set; }
        [DataMember(Order = 29)] public bool PasswordNeverExpires { get; set; }
        [DataMember(Order = 30)] public bool CanNotChangePassword { get; set; }
        [DataMember(Order = 31)] public bool ChangePasswordAtLogon { get; set; }
        [DataMember(Order = 32)] public string AccountPassword { get; set; }
        [DataMember(Order = 33)] public DateTime AccountExpirationDate { get; set; }
        [DataMember(Order = 34)] public string DirectReports { get; set; }
        [DataMember(Order = 35)] public string MemberOf { get; set; }
        [DataMember(Order = 36)] public string UserAccountControl { get; set; }
    }
}

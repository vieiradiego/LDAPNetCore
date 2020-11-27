using AgentNetCore.Data.Converter;
using AgentNetCore.Data.VO;
using AgentNetCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgentNetCore.Data.Converters
{
   
    public class UserConverter : IParser<UserVO, User>, IParser<User, UserVO>
    {
        GroupConverter _groupConverter = new GroupConverter();
        public User Parse(UserVO origin)
        {
            if (origin == null) return null;
            return new User()
            {
                Name = origin.Name,
                Surname = origin.Surname,
                LogonName = origin.LogonName,
                FirstName = origin.FirstName,
                DisplayName = origin.DisplayName,
                Inicials = origin.Inicials,
                OfficePhone = origin.OfficePhone,
                MobilePhone = origin.MobilePhone,
                Description = origin.Description,
                Office = origin.Office,
                EmailAddress = origin.EmailAddress,
                PostalCode = origin.PostalCode,
                Country = origin.Country,
                City = origin.City,
                State = origin.State,
                StreetAddress = origin.StreetAddress,
                Title = origin.Title,
                Departament = origin.Departament,
                Company = origin.Company,
                Manager = origin.Manager,
                EmployeeID = origin.EmployeeID,
                Enabled = origin.Enabled,
                AuthType = origin.Surname,
                PasswordNotRequired = origin.PasswordNotRequired,
                PasswordNeverExpires = origin.PasswordNeverExpires,
                CanNotChangePassword = origin.CanNotChangePassword,
                ChangePasswordAtLogon = origin.ChangePasswordAtLogon,
                AccountPassword = origin.AccountPassword,
                AccountExpirationDate = origin.AccountExpirationDate,
                PathDomain = origin.PathDomain,
                Domain = origin.Domain,
                SamAccountName = origin.SamAccountName,
                DistinguishedName = origin.DistinguishedName,
                DirectReports = origin.DirectReports,
                UserAccountControl = origin.UserAccountControl,
            };
        }

        public UserVO Parse(User origin)
        {
            if (origin == null) return null;
            return new UserVO()
            {
                Name = origin.Name,
                Surname = origin.Surname,
                LogonName = origin.LogonName,
                FirstName = origin.FirstName,
                DisplayName = origin.DisplayName,
                Inicials = origin.Inicials,
                OfficePhone = origin.OfficePhone,
                MobilePhone = origin.MobilePhone,
                Description = origin.Description,
                Office = origin.Office,
                EmailAddress = origin.EmailAddress,
                PostalCode = origin.PostalCode,
                Country = origin.Country,
                City = origin.City,
                State = origin.State,
                StreetAddress = origin.StreetAddress,
                Title = origin.Title,
                Departament = origin.Departament,
                Company = origin.Company,
                Manager = origin.Manager,
                EmployeeID = origin.EmployeeID,
                Enabled = origin.Enabled,
                AuthType = origin.Surname,
                PasswordNotRequired = origin.PasswordNotRequired,
                PasswordNeverExpires = origin.PasswordNeverExpires,
                CanNotChangePassword = origin.CanNotChangePassword,
                ChangePasswordAtLogon = origin.ChangePasswordAtLogon,
                AccountPassword = origin.AccountPassword,
                AccountExpirationDate = origin.AccountExpirationDate,
                PathDomain = origin.PathDomain,
                Domain = origin.Domain,
                SamAccountName = origin.SamAccountName,
                DistinguishedName = origin.DistinguishedName,
                DirectReports = origin.DirectReports,
                UserAccountControl = origin.UserAccountControl,
            };
        }

        public List<User> ParseList(List<UserVO> origin)
        {
            if (origin == null) return null;
            return origin.Select(item => Parse(item)).ToList();
        }

        public List<UserVO> ParseList(List<User> origin)
        {
            return origin.Select(item => Parse(item)).ToList();
        }
    }
}


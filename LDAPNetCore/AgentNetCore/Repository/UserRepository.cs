using Persistence.Interface;
using Persistence.Model;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AgentNetCore.Business
{
    public class UserRepository: IUserService
    { 
     private volatile int count;

    public User Create(User user)
    {
        return user;
    }

    public void Delete(long id)
    {

    }

    public List<User> FindAll()
    {
        List<User> users = new List<User>();
        for (int i = 0; i < 8; i++)
        {
            User user = MockUsers(i);
            users.Add(user);
        }
        return users;
    }

    private User MockUsers(int i)
    {
        return new User
        {
            Id = IncrementAndGet(),
            Name = "User Name" + i,
            FirstName = "User First Name" + i,
            LogonName = "Logon Name" + i,
            DisplayName = "Display Name" + i,
            Inicials = "Inicials" + i,
            OfficePhone = "Office Phone" + i,
            MobilePhone = "Office Phone" + i,
            Description = "Office Phone" + i,
            Organization = "Office Phone" + i,
            Office = "Office Phone" + i,
            EmailAddress = "Office Phone" + i,
            PostalCode = "Office Phone" + i,
            Country = "Office Phone" + i,
            City = "Office Phone" + i,
            State = "Office Phone" + i,
            StreetAddress = "Office Phone" + i,
            Title = "Office Phone" + i,
            Departament = "Office Phone" + i,
            Manager = "Office Phone" + i,
            EmployeeID = "Office Phone" + i,
            Enabled = true,
            Company = "Office Phone" + i,
            AuthType = "Office Phone" + i,
            PasswordNotRequired = false,
            PasswordNeverExpires = true,
            CanNotChangePassword = false,
            ChangePasswordAtLogon = false,
            AccountPassword = "Pass" + i,
            AccountExpirationDate = DateTime.Now,
            PathDomain = "Path" + i,
            SamAccountName = "Sam" + i
        };
    }

    private long IncrementAndGet()
    {
        return Interlocked.Increment(ref count);
    }

    public User FindById(long id)
    {
        return new User
        {
            Id = 1,
            Name = "User Name",
            FirstName = "User First Name",
            LogonName = "Logon Name",
            DisplayName = "Display Name",
            Inicials = "Inicials",
            OfficePhone = "Office Phone",
            MobilePhone = "Office Phone",
            Description = "Office Phone",
            Organization = "Office Phone",
            Office = "Office Phone",
            EmailAddress = "Office Phone",
            PostalCode = "Office Phone",
            Country = "Office Phone",
            City = "Office Phone",
            State = "Office Phone",
            StreetAddress = "Office Phone",
            Title = "Office Phone",
            Departament = "Office Phone",
            Manager = "Office Phone",
            EmployeeID = "Office Phone",
            Enabled = true,
            Company = "Office Phone",
            AuthType = "Office Phone",
            PasswordNotRequired = false,
            PasswordNeverExpires = true,
            CanNotChangePassword = false,
            ChangePasswordAtLogon = false,
            AccountPassword = "Pass",
            AccountExpirationDate = DateTime.Now,
            PathDomain = "Path",
            SamAccountName = "Sam"
        };
    }

    public User Update(User user)
    {
        return user;
    }
}
}

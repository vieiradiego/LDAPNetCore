using AgentNetCore.Model;
using System.Collections.Generic;

namespace AgentNetCore.Service
{
    public interface IUserService
    {
        User Create(User user);
        User Update(User user);
        User FindByEmail(string email);
        List<User> FindAll();
        void Delete(string domain, string samName);
    }
}

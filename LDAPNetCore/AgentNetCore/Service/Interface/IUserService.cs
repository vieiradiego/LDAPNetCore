using AgentNetCore.Model;
using System.Collections.Generic;

namespace AgentNetCore.Service
{
    public interface IUserService
    {
        User Create(User user);
        User Update(User user);
        User FindByEmail(string domain, string email);
        List<User> FindAll(string domain);
        void Delete(string domain, string email);
    }
}

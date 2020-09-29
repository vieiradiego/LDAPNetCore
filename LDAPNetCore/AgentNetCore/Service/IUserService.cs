using AgentNetCore.Model;
using System.Collections.Generic;

namespace AgentNetCore.Service
{
    public interface IUserService
    {
        User Create(User user);
        User Update(User user);
        User FindById(long id);
        List<User> FindAll();
        void Delete(long id);
    }
}

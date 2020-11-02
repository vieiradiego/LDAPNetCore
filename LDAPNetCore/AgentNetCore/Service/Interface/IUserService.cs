using AgentNetCore.Data.VO;
using System.Collections.Generic;

namespace AgentNetCore.Service
{
    public interface IUserService
    {
        UserVO Create(UserVO user);
        UserVO Update(UserVO user);
        UserVO FindByEmail(string email);
        List<UserVO> FindAll();
        void Delete(string domain, string samName);
    }
}

using AgentNetCore.Data.VO;
using AgentNetCore.Service.Interface;
using System.Collections.Generic;

namespace AgentNetCore.Service
{
    public interface IUserService : IService<IUserService>
    {
        UserVO Create(UserVO user);
        
        List<UserVO> FindAll();
        UserVO FindByEmail(string dn, string email);
        UserVO FindBySamName(string dn, string samName);
        UserVO FindByName(string dn, string firstName,string lastName);
        UserVO FindByFirstName(string dn, string firstName);
        UserVO FindByLastName(string dn, string lastName);
        UserVO Update(UserVO user);
        void Delete(string domain, string samName);
    }
}

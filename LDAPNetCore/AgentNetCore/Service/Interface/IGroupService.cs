using AgentNetCore.Data.VO;
using AgentNetCore.Service.Interface;
using System.Collections.Generic;

namespace AgentNetCore.Service
{
    public interface IGroupService : IService
    {
        GroupVO Create(GroupVO group);
        GroupVO Update(GroupVO group);
        GroupVO FindByEmail(string domain, string email);
        GroupVO FindBySamName(string domain, string samName);
        List<GroupVO> FindAll();
        void Delete(string domain, string samName);
    }
}

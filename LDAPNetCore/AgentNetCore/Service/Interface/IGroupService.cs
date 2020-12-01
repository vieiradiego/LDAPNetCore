using AgentNetCore.Data.VO;
using AgentNetCore.Service.Interface;
using System.Collections.Generic;

namespace AgentNetCore.Service
{
    public interface IGroupService : IService<IGroupService>
    {
        GroupVO Create(GroupVO group);
        List<GroupVO> FindAll();
        List<GroupVO> FindByDn(string dn);
        GroupVO FindByEmail(string domain, string email);
        GroupVO FindBySamName(string domain, string samName);
        GroupVO Update(GroupVO group);
        bool Delete(string domain, string samName);
        bool AddUser(string userDn, string groupDn);
        bool RemoveUser(string userDn, string groupDn);
        bool ChangeGroup(string userDn, string newGroupDn, string oldGroupDn);
        List<GroupVO> GetGroups(string userDn);
        
    }
}

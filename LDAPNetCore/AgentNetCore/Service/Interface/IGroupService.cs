using AgentNetCore.Model;
using System.Collections.Generic;

namespace AgentNetCore.Service
{
    public interface IGroupService
    {
        Group Create(Group group);
        Group Update(Group group);
        Group FindByEmail(string domain, string email);
        Group FindBySamName(string domain, string samName);
        List<Group> FindAll();
        void Delete(string domain, string samName);
    }
}

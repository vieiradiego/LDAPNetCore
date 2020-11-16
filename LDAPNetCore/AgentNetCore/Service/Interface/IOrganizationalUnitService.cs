using AgentNetCore.Data.VO;
using AgentNetCore.Service.Interface;
using System.Collections.Generic;

namespace AgentNetCore.Service
{
    public interface IOrganizationalUnitService : IService<IOrganizationalUnitService>
    {
        OrganizationalUnitVO Create(OrganizationalUnitVO organizationalUnit);
        OrganizationalUnitVO Update(OrganizationalUnitVO organizationalUnit);
        OrganizationalUnitVO FindByName(string domain, string nameOU);
        List<OrganizationalUnitVO> FindAll(string domain);
        void Delete(string domain, string samName);
    }
}

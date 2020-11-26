using AgentNetCore.Data.VO;
using AgentNetCore.Service.Interface;
using System.Collections.Generic;

namespace AgentNetCore.Service
{
    public interface IOrganizationalUnitService : IService<IOrganizationalUnitService>
    {
        OrganizationalUnitVO Create(OrganizationalUnitVO organizationalUnit);
        List<OrganizationalUnitVO> FindAll();
        List<OrganizationalUnitVO> FindByDn(string dn);
        OrganizationalUnitVO FindByName(string domain, string nameOU);
        OrganizationalUnitVO Update(OrganizationalUnitVO organizationalUnit);
        void Delete(string domain, string samName);
    }
}

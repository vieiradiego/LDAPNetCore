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
        OrganizationalUnitVO FindByName(string dn, string name);
        OrganizationalUnitVO FindByOu(string dn, string ou);
        OrganizationalUnitVO Update(OrganizationalUnitVO organizationalUnit);
        bool Delete(string domain, string samName);
    }
}

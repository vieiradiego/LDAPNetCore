using AgentNetCore.Model;
using System.Collections.Generic;

namespace AgentNetCore.Service
{
    public interface IOrganizationalUnitService
    {
        OrganizationalUnit Create(OrganizationalUnit organizationalUnit);
        OrganizationalUnit Update(OrganizationalUnit organizationalUnit);
        OrganizationalUnit FindByName(string domain, string nameOU);
        List<OrganizationalUnit> FindAll(string domain);
        void Delete(string domain, string samName);
    }
}

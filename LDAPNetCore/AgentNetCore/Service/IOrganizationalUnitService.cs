using AgentNetCore.Model;
using System.Collections.Generic;

namespace AgentNetCore.Service
{
    public interface IOrganizationalUnitService
    {
        OrganizationalUnit Create(OrganizationalUnit organizationalUnit);
        OrganizationalUnit Update(Group organizationalUnit);
        OrganizationalUnit FindBySamName(string samName);
        List<OrganizationalUnit> FindAll();
        void Delete(string samName);
    }
}

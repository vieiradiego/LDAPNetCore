using AgentNetCore.Model;
using System.Collections.Generic;

namespace AgentNetCore.Service
{
    public interface IOrganizationalUnitService
    {
        OrganizationalUnit Create(OrganizationalUnit organizationalUnit);
        OrganizationalUnit Update(Group organizationalUnit);
        OrganizationalUnit FindBySamName(string domain, string samName);
        List<OrganizationalUnit> FindAll(string domain);
        void Delete(string domain, string samName);
    }
}

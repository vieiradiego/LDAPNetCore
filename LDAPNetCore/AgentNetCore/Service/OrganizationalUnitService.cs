using AgentNetCore.Context;
using AgentNetCore.Model;
using System;
using System.Collections.Generic;


namespace AgentNetCore.Service
{
    public class OrganizationalUnitService : IOrganizationalUnitService
    {
        private MySQLContext _mySQLContext;
        public OrganizationalUnitService(MySQLContext mySQLContext)
        {
            _mySQLContext = mySQLContext;
        }
        public OrganizationalUnit Create(OrganizationalUnit organizationUnit)
        {
            try
            {
                OrganizationalUnitRepository ldapOrg = new OrganizationalUnitRepository(_mySQLContext);
                return ldapOrg.Create(organizationUnit);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<OrganizationalUnit> FindAll(string domain)
        {
            OrganizationalUnitRepository ldapOrg = new OrganizationalUnitRepository(_mySQLContext);
            return ldapOrg.FindAll(domain);
        }


        public OrganizationalUnit FindByName(string domain, string nameOU)
        {
            OrganizationalUnitRepository ldapOrg = new OrganizationalUnitRepository(_mySQLContext);
            return ldapOrg.FindByName(domain, nameOU);
        }
        public OrganizationalUnit Update(OrganizationalUnit organizationUnit)
        {
            try
            {
                OrganizationalUnitRepository ldapOrg = new OrganizationalUnitRepository(_mySQLContext);
                return ldapOrg.Update(organizationUnit);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Exist(string domain, string nameOU)
        {
            OrganizationalUnitRepository ldapOrg = new OrganizationalUnitRepository(_mySQLContext);
            if (ldapOrg.FindByName(domain, nameOU) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Delete(string domain, string nameOU)
        {
            OrganizationalUnitRepository ldapOrg = new OrganizationalUnitRepository(_mySQLContext);
            OrganizationalUnit result = new OrganizationalUnit();
            result = ldapOrg.FindByName(domain, nameOU);
            try
            {
                if (result != null)
                {

                    ldapOrg.Delete(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

using AgentNetCore.Context;
using AgentNetCore.Data.Converters;
using AgentNetCore.Data.VO;
using AgentNetCore.Model;
using System;
using System.Collections.Generic;


namespace AgentNetCore.Service
{
    public class OrganizationalUnitService : IOrganizationalUnitService
    {
        private readonly MySQLContext _mySQLContext;
        private readonly OrganizationalUnitConverter _converter;
        public OrganizationalUnitService(MySQLContext mySQLContext)
        {
            _mySQLContext = mySQLContext;
            _converter = new OrganizationalUnitConverter();
        }
        public OrganizationalUnitVO Create(OrganizationalUnitVO organizationalUnit)
        {
            try
            {
                OrganizationalUnitRepository ldapOrg = new OrganizationalUnitRepository(_mySQLContext);
                var organizationalUnitEntity = _converter.Parse(organizationalUnit);
                organizationalUnitEntity = ldapOrg.Create(organizationalUnitEntity);
                return _converter.Parse(organizationalUnitEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<OrganizationalUnitVO> FindAll(string domain)
        {
            OrganizationalUnitRepository ldapOrg = new OrganizationalUnitRepository(_mySQLContext);
            return _converter.ParseList(ldapOrg.FindAll(domain));
        }


        public OrganizationalUnitVO FindByName(string domain, string nameOU)
        {
            OrganizationalUnitRepository ldapOrg = new OrganizationalUnitRepository(_mySQLContext);
            return _converter.Parse(ldapOrg.FindByName(domain, nameOU));
        }
        public OrganizationalUnitVO Update(OrganizationalUnitVO organizationalUnit)
        {
            try
            {
                OrganizationalUnitRepository ldapOrg = new OrganizationalUnitRepository(_mySQLContext);
                var organizationalUnitEntity = _converter.Parse(organizationalUnit);
                organizationalUnitEntity = ldapOrg.Update(organizationalUnitEntity);
                return  _converter.Parse(organizationalUnitEntity);
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

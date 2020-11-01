using AgentNetCore.Context;
using AgentNetCore.Model;
using System;
using System.Collections.Generic;

namespace AgentNetCore.Service
{
    public class GroupService : IGroupService
    {
        private MySQLContext _mySQLContext;
        public GroupService(MySQLContext mySQLContext)
        {
            _mySQLContext = mySQLContext;
        }
        public Group Create(Group group)
        {
            try
            {
                GroupRepository ldapGroup = new GroupRepository(_mySQLContext);
                return ldapGroup.Create(group);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Group> FindAll()
        {
            GroupRepository ldapGroup = new GroupRepository(_mySQLContext);
            return ldapGroup.FindAll();
        }

        public Group FindBySamName(string domain, string samName)
        {
            GroupRepository ldapGroup = new GroupRepository(_mySQLContext);
            return ldapGroup.FindBySamName(domain, samName);
        }

        public Group FindByEmail(string domain, string email)
        {
            GroupRepository ldapGroup = new GroupRepository(_mySQLContext);
            return ldapGroup.FindByEmail(domain, email);
        }

        public Group Update(Group group)
        {
            try
            {
                GroupRepository ldapGroup = new GroupRepository(_mySQLContext);
                return ldapGroup.Update(group);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Exist(string domain, string samName)
        {
            GroupRepository ldapGroup = new GroupRepository(_mySQLContext);
            if (ldapGroup.FindBySamName(domain, samName) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Delete(string domain, string samName)
        {
            GroupRepository ldapGroup = new GroupRepository(_mySQLContext);
            Group result = new Group();
            result = ldapGroup.FindBySamName(domain, samName);
            try
            {
                if (result != null)
                {

                    ldapGroup.Delete(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

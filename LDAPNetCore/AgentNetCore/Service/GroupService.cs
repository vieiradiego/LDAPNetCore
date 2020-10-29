using AgentNetCore.Context;
using AgentNetCore.Model;
using System;
using System.Collections.Generic;

namespace AgentNetCore.Service
{
    public class GroupService : IGroupService
    {
        public GroupService(MySQLContext context)
        {

        }
        public Group Create(Group group)
        {
            try
            {
                GroupRepository ldapGroup = new GroupRepository();
                return ldapGroup.Create(group);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Group> FindAll(string domain)
        {
            GroupRepository ldapGroup = new GroupRepository();
            return ldapGroup.FindAll(domain);
        }

        public Group FindBySamName(string domain, string name)
        {
            GroupRepository ldapGroup = new GroupRepository();
            return ldapGroup.FindBySamName(domain, name);
        }

        public Group FindByEmail(string domain, string email)
        {
            GroupRepository ldapGroup = new GroupRepository();
            return ldapGroup.FindByEmail(domain, email);
        }

        public Group Update(Group group)
        {
            try
            {
                if (group != null)
                {
                    GroupRepository ldapGroup = new GroupRepository();
                    ldapGroup.Update(group);
                }
                else
                {
                    return new Group();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return group;
        }

        private bool Exist(string domain, string email)
        {
            GroupRepository ldapGroup = new GroupRepository();
            if (ldapGroup.FindByEmail(domain, email) != null)
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
            GroupRepository ldapGroup = new GroupRepository();
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

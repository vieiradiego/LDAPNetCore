using AgentNetCore.Context;
using AgentNetCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgentNetCore.Service
{
    public class GroupService : IGroupService
    {
        private MySQLContext _context;
        private LDAPGroup _ldapGroup;

        public GroupService(MySQLContext context)
        {
            _context = context;
            _ldapGroup = new LDAPGroup();
        }

        public Group Create(Group group)
        {
            try
            {
                return _ldapGroup.Create(group);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Group> FindAll()
        {
            return _ldapGroup.FindAll("", "");
        }

        public Group FindBySamName(string name)
        {
            return _ldapGroup.FindBySamName(name);
        }

        public Group FindByEmail(string email)
        {
            return _ldapGroup.FindByEmail(email);
        }

        public Group Update(Group group)
        {
            try
            {
                if (group != null)
                {
                    _ldapGroup.Update(group);
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

        private bool Exist(string email)
        {
            if (_ldapGroup.FindByEmail(email) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Delete(string samName)
        {
            var result = _ldapGroup.FindBySamName(samName);
            try
            {
                if (result != null)
                {
                    _ldapGroup.Delete(FindBySamName(samName));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

using AgentNetCore.Context;
using AgentNetCore.Data.Converters;
using AgentNetCore.Data.VO;
using AgentNetCore.Model;
using System;
using System.Collections.Generic;

namespace AgentNetCore.Service
{
    public class GroupService : IGroupService
    {
        private MySQLContext _mySQLContext;
        private readonly GroupConverter _converter;
        public GroupService(MySQLContext mySQLContext)
        {
            _mySQLContext = mySQLContext;
            _converter = new GroupConverter();
        }
        public GroupVO Create(GroupVO group)
        {
            try
            {
                GroupRepository ldapGroup = new GroupRepository(_mySQLContext);
                var GroupEntity = _converter.Parse(group);
                GroupEntity = ldapGroup.Create(GroupEntity);
                return _converter.Parse(GroupEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GroupVO> FindAll()
        {
            GroupRepository ldapGroup = new GroupRepository(_mySQLContext);
            return _converter.ParseList(ldapGroup.FindAll());
        }

        public GroupVO FindBySamName(string domain, string samName)
        {
            GroupRepository ldapGroup = new GroupRepository(_mySQLContext);
            return _converter.Parse(ldapGroup.FindBySamName(domain, samName));
        }

        public GroupVO FindByEmail(string domain, string email)
        {
            GroupRepository ldapGroup = new GroupRepository(_mySQLContext);
            return _converter.Parse(ldapGroup.FindByEmail(domain, email));
        }

        public GroupVO Update(GroupVO group)
        {
            try
            {
                GroupRepository ldapGroup = new GroupRepository(_mySQLContext);
                var groupEntity = _converter.Parse(group);
                groupEntity = ldapGroup.Update(groupEntity);
                return _converter.Parse(groupEntity);
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

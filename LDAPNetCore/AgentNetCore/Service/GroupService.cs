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
        private readonly MySQLContext _mySQLContext;
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
                return null;
            }
        }
        public List<GroupVO> FindAll()
        {
            GroupRepository ldapGroup = new GroupRepository(_mySQLContext);
            return _converter.ParseList(ldapGroup.FindAll());
        }
        public GroupVO FindBySamName(string dn, string samName)
        {
            GroupRepository ldapGroup = new GroupRepository(_mySQLContext);
            return _converter.Parse(ldapGroup.FindBySamName(dn, samName));
        }

        public GroupVO FindByEmail(string dn, string email)
        {
            GroupRepository ldapGroup = new GroupRepository(_mySQLContext);
            return _converter.Parse(ldapGroup.FindByEmail(dn, email));
        }
        public List<GroupVO> FindByDn(string dn)
        {
            GroupRepository ldapGroup = new GroupRepository(_mySQLContext);
            return _converter.ParseList(ldapGroup.FindByDn(dn));
        }
        private bool Exist(string dn, string samName)
        {
            GroupRepository ldapGroup = new GroupRepository(_mySQLContext);
            if (ldapGroup.FindBySamName(dn, samName) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
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
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        public void Delete(string dn, string samName)
        {
            GroupRepository ldapGroup = new GroupRepository(_mySQLContext);
            Group result = new Group();
            result = ldapGroup.FindBySamName(dn, samName);
            try
            {
                if (result != null)
                {

                    ldapGroup.Delete(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }
        }
    }
}

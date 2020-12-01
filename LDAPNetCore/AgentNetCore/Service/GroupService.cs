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
        public bool Delete(string dn, string samName)
        {
            GroupRepository ldapGroup = new GroupRepository(_mySQLContext);
            CredentialRepository credential = new CredentialRepository(_mySQLContext);
            Group result = new Group();
            credential.DN = dn;
            result = ldapGroup.FindBySamName(credential, samName);
            try
            {
                if (result != null)
                {
                    if (ldapGroup.Delete(credential, result))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return false;
            }
        }
        public bool AddUser(string userDn, string groupDn)
        {
            GroupRepository ldapGroup = new GroupRepository(_mySQLContext);
            return ldapGroup.AddUser(userDn, groupDn);
        }
        public bool RemoveUser(string userDn, string groupDn)
        {
            GroupRepository ldapGroup = new GroupRepository(_mySQLContext);
            return ldapGroup.RemoveUser(userDn, groupDn);
        }

        public bool ChangeGroup(string userDn, string newGroupDn, string oldGroupDn)
        {
            GroupRepository ldapGroup = new GroupRepository(_mySQLContext);
            if ((ldapGroup.AddUser(userDn, newGroupDn)) && (ldapGroup.RemoveUser(userDn, oldGroupDn)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<GroupVO> GetGroups(string userDn)
        {
            UserRepository ldapUser = new UserRepository(_mySQLContext);
            CredentialRepository credential = new CredentialRepository(_mySQLContext);
            credential.DN = userDn;
            return _converter.ParseList(ldapUser.GetGroups(credential, userDn));
        }
    }
}

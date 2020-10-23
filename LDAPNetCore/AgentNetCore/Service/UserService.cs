using AgentNetCore.Context;
using AgentNetCore.Model;
using AgentNetCore.Repository;
using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Threading;

namespace AgentNetCore.Service
{
    public class UserService : IUserService
    {
        private MySQLContext _context;
        private LDAPUser _ldapUser;

        public UserService(MySQLContext context)
        {
            _context = context;
            _ldapUser = new LDAPUser();
        }

        public User Create(User user)
        {
            try
            {
                user = _ldapUser.Create(user);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }
            return user;
        }

        public List<User> FindAll()
        {
            return _ldapUser.FindAll("", "");
        }

        public User FindByName(string name)
        {
            return _ldapUser.FindByName(name);
        }

        public User FindByEmail(string email)
        {
            return _ldapUser.FindByEmail(email);
        }

        public User Update(User user)
        {
            try
            {
                if (user != null)
                {
                    _ldapUser.Update(user);
                }
                else
                {
                    return new User();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return user;
        }

        private bool Exist(string email)
        {
            if (_ldapUser.FindByEmail(email) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Delete(string email)
        {
            var result = _ldapUser.FindByEmail(email);
            try
            {
                if (result != null)
                {
                    _ldapUser.Delete(FindByEmail(email));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

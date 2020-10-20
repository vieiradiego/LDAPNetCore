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
            return _ldapUser.FindAll("","");
        }

        public User FindByName(string name)
        {
            return _ldapUser.FindByEmail(name);
        }

        public User FindByEmail(string email)
        {
            return _ldapUser.FindByEmail(email);
        }

        public User Update(User user)
        {
            if (!Exist(user.Id)) return new User();
            var result = _context.Users.SingleOrDefault(p => p.Id.Equals(user.Id));
            try
            {
                _ldapUser.Update(user);
                //MySql
                //_context.Entry(result).CurrentValues.SetValues(user);
                //_context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return user;
        }

        private bool Exist(long? id)
        {
            return _context.Users.Any(p => p.Id.Equals(id));
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

using AgentNetCore.Context;
using AgentNetCore.Model;
using AgentNetCore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AgentNetCore.Service
{
    public class UserService : IUserService
    {
        private MySQLContext _context;
        private LDAPContext _LDAPContext;
        
        public UserService(MySQLContext context)
        {
            _context = context;
            _LDAPContext = new LDAPContext();
        }

        public User Create(User user)
        {
            try
            {
                _context.Add(user);
                _LDAPContext.Create(user);
                Console.WriteLine("User Added successfully");
                _context.SaveChanges();
                Console.WriteLine("Saved User successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }
            return user;
        }
                
        public List<User> FindAll()
        {
            _LDAPContext.FindAll();
            return _context.Users.ToList();
        }

        public User FindById(long id)
        {
            return _context.Users.SingleOrDefault(p => p.Id.Equals(id));
        }

        public User Update(User user)
        {
            if (!Exist(user.Id)) return new User();

            var result = _context.Users.SingleOrDefault(p => p.Id.Equals(user.Id));
            try
            {
                _context.Entry(result).CurrentValues.SetValues(user);
                _context.SaveChanges();
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
        public void Delete(long id)
        {
            var result = _context.Users.SingleOrDefault(p => p.Id.Equals(id));
            try
            {
                if (result != null) _context.Users.Remove(result);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

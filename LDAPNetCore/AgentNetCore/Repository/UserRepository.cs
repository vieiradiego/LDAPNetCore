using AgentNetCore.Context;
using AgentNetCore.Model;
using AgentNetCore.Service;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AgentNetCore.Repository
{
    
    public class UserRepository : IUserService
    {
        private MySQLContext _context;

        public UserRepository(MySQLContext context)
        {
            _context = context;
        }
        public User Create(User user)
        {
            try
            {
                _context.Add(user);
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

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        public List<User> FindAll()
        {
            throw new NotImplementedException();
        }

        public User FindById(long id)
        {
            throw new NotImplementedException();
        }

        public User Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}

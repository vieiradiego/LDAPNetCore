using AgentNetCore.Context;
using AgentNetCore.Model;
using System;
using System.Collections.Generic;

namespace AgentNetCore.Service
{
    public class UserService : IUserService
    {
        private readonly MySQLContext _mySQLContext;

        public UserService(MySQLContext mySQLContext)
        {
            _mySQLContext = mySQLContext;
        }
        public User Create(User user)
        {
            try
            {
                UserRepository ldapUser = new UserRepository(_mySQLContext);
                user = ldapUser.Create(user);
                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }
            return null;
        }

        public List<User> FindAll()
        {
            UserRepository ldapUser = new UserRepository(_mySQLContext);
            return ldapUser.FindAll();
        }

        public User FindByName(string domain, string name)
        {
            UserRepository ldapUser = new UserRepository(_mySQLContext);
            return ldapUser.FindByName(domain, name);
        }

        public User FindByEmail(string domain, string email)
        {
            UserRepository ldapUser = new UserRepository(_mySQLContext);
            return ldapUser.FindByEmail(domain, email);
        }

        public User FindByEmail(string email)
        {
            UserRepository ldapUser = new UserRepository(_mySQLContext);
            return ldapUser.FindByEmail(email);
        }

        public User Update(User user)
        {
            try
            {
                UserRepository userRepo = new UserRepository(_mySQLContext);
                userRepo.Update(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return user;
        }

        private bool Exist(string domain, string samName)
        {
            UserRepository ldapUser = new UserRepository(_mySQLContext);
            if (ldapUser.FindBySamName(domain, samName) != null)
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
            UserRepository ldapUser = new UserRepository(_mySQLContext);
            User result = new User();
            result = ldapUser.FindBySamName(domain, samName);
            try
            {
                if (result != null)
                {
                    ldapUser.Delete(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

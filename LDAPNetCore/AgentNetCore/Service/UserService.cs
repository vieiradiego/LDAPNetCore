using AgentNetCore.Context;
using AgentNetCore.Model;
using System;
using System.Collections.Generic;

namespace AgentNetCore.Service
{
    public class UserService : IUserService
    {
        public UserService()
        {
            
        }        
        public User Create(User user)
        {
            try
            {
                UserRepository ldapUser = new UserRepository();
                user = ldapUser.Create(user);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }
            return user;
        }

        public List<User> FindAll(string domain)
        {
            UserRepository ldapUser = new UserRepository();
            return ldapUser.FindAll(domain);
        }

        public User FindByName(string domain, string name)
        {
            UserRepository ldapUser = new UserRepository();
            return ldapUser.FindByName(domain, name);
        }

        public User FindByEmail(string domain, string email)
        {
            UserRepository ldapUser = new UserRepository();
            return ldapUser.FindByEmail(domain, email);
        }

        public User Update(User user)
        {
            try
            {
                if (user != null)
                {
                    UserRepository ldapUser = new UserRepository();
                    ldapUser.Update(user);
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

        private bool Exist(string domain, string email)
        {
            UserRepository ldapUser = new UserRepository();
            if (ldapUser.FindByEmail(domain, email) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Delete(string domain, string email)
        {
            UserRepository ldapUser = new UserRepository();
            User result = new User();
            result = ldapUser.FindByEmail(domain, email);
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

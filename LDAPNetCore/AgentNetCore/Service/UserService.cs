using AgentNetCore.Context;
using AgentNetCore.Data.Converters;
using AgentNetCore.Data.VO;
using AgentNetCore.Model;
using System;
using System.Collections.Generic;

namespace AgentNetCore.Service
{
    public class UserService : IUserService
    {
        private readonly MySQLContext _mySQLContext;
        private readonly UserConverter _converter;
        public UserService(MySQLContext mySQLContext)
        {
            _mySQLContext = mySQLContext;
            _converter = new UserConverter();
        }
        public UserVO Create(UserVO user)
        {
            try
            {
                UserRepository ldapUser = new UserRepository(_mySQLContext);
                var userEntity = _converter.Parse(user);
                userEntity = ldapUser.Create(userEntity);
                return _converter.Parse(userEntity);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
            }
            return null;
        }

        public List<UserVO> FindAll()
        {
            UserRepository ldapUser = new UserRepository(_mySQLContext);
            return _converter.ParseList(ldapUser.FindAll());
        }

        public UserVO FindByName(string domain, string name)
        {
            UserRepository ldapUser = new UserRepository(_mySQLContext);
            return _converter.Parse(ldapUser.FindByName(domain, name));
        }

        public UserVO FindByEmail(string domain, string email)
        {
            UserRepository ldapUser = new UserRepository(_mySQLContext);
            return _converter.Parse(ldapUser.FindByEmail(domain, email));
        }

        public UserVO FindByEmail(string email)
        {
            UserRepository ldapUser = new UserRepository(_mySQLContext);
            return _converter.Parse(ldapUser.FindByEmail(email));
        }

        public UserVO Update(UserVO user)
        {
            try
            {
                UserRepository ldapUser = new UserRepository(_mySQLContext);
                var userEntity = _converter.Parse(user);
                userEntity = ldapUser.Update(userEntity);
                return _converter.Parse(userEntity);
            }
            catch (Exception)
            {
                return null;
            }
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

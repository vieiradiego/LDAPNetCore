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

        public UserVO FindByEmail(string dn, string email)
        {
            UserRepository ldapUser = new UserRepository(_mySQLContext);
            return _converter.Parse(ldapUser.FindByEmail(dn, email));
        }

        public UserVO FindBySamName(string dn, string samName)
        {
            UserRepository ldapUser = new UserRepository(_mySQLContext);

            return _converter.Parse(ldapUser.FindBySamName(dn, samName));
        }

        public UserVO FindByName(string dn, string firstName, string lastName)
        {
            UserRepository ldapUser = new UserRepository(_mySQLContext);
            return _converter.Parse(ldapUser.FindByName(dn, firstName + " " + lastName));
        }

        public UserVO FindByFirstName(string dn, string firstName)
        {
            UserRepository ldapUser = new UserRepository(_mySQLContext);
            return _converter.Parse(ldapUser.FindByFirstName(dn, firstName));
        }

        public UserVO FindByLastName(string dn, string lastName)
        {
            UserRepository ldapUser = new UserRepository(_mySQLContext);
            return _converter.Parse(ldapUser.FindByLastName(dn, lastName));
        }

        public List<UserVO> FindByDn(string dn)
        {
            UserRepository ldapUser = new UserRepository(_mySQLContext);
            return _converter.ParseList(ldapUser.FindByDn(dn));
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
        public void Delete(string dn, string samName)
        {
            UserRepository ldapUser = new UserRepository(_mySQLContext);
            CredentialRepository credential = new CredentialRepository(_mySQLContext);
            User result = new User();
            credential.DN = dn;
            result = ldapUser.FindBySamName(credential, samName);
            try
            {
                if (result != null)
                {
                    ldapUser.Delete(credential, result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

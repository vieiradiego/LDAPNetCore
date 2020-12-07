using AgentNetCore.Context;
using AgentNetCore.Data.Converters;
using AgentNetCore.Data.VO;
using AgentNetCore.Model;
using System;
using System.Collections.Generic;
using System.Security;

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

        private bool Exist(string dn, string samName)
        {
            try
            {
                UserRepository ldapUser = new UserRepository(_mySQLContext);
                User result = new User();
                result = ldapUser.FindBySamName(dn, samName);
                if (result != null)
                {
                    if ((!String.IsNullOrWhiteSpace(result.SamAccountName)) &&
                        (!String.IsNullOrWhiteSpace(result.DistinguishedName)))
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
        public bool Delete(string dn, string samName)
        {
            UserRepository ldapUser = new UserRepository(_mySQLContext);
            CredentialRepository credential = new CredentialRepository(_mySQLContext);
            User result = new User();
            credential.DN = dn;
            result = ldapUser.FindBySamName(credential, samName);
            try
            {
                if (Exist(dn, samName))
                {
                    return (ldapUser.Delete(credential, result));
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
        public UserVO Inactive(string dn, string samName)
        {
            try
            {
                if (Exist(dn, samName))
                {
                    UserRepository ldapUser = new UserRepository(_mySQLContext);
                    CredentialRepository credential = new CredentialRepository(_mySQLContext);
                    credential.DN = dn;
                    return _converter.Parse(ldapUser.Disable(credential, samName));
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        public UserVO Active(string dn, string samName)
        {
            try
            {
                if (Exist(dn, samName))
                {
                    UserRepository ldapUser = new UserRepository(_mySQLContext);
                    CredentialRepository credential = new CredentialRepository(_mySQLContext);
                    credential.DN = dn;
                    return _converter.Parse(ldapUser.Enable(credential, samName));
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        public bool ResetPass(string dn, string samName, string pass)
        {
            if (Exist(dn, samName))
            {
                UserRepository ldapUser = new UserRepository(_mySQLContext);
                CredentialRepository credential = new CredentialRepository(_mySQLContext);
                credential.DN = dn;
                return (ldapUser.ResetPassBySamName(credential, samName, pass));
            }
            else
            {
                return false;
            }
        }
        public List<UserVO> GetUsers(string groupDn)
        {
            GroupRepository ldapGroup = new GroupRepository(_mySQLContext);
            return _converter.ParseList(ldapGroup.GetUsers(groupDn));
        }
    }
}

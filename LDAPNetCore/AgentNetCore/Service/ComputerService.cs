using AgentNetCore.Context;
using AgentNetCore.Data.Converters;
using AgentNetCore.Data.VO;
using AgentNetCore.Model;
using System;
using System.Collections.Generic;

namespace AgentNetCore.Service
{
    public class ComputerService : IComputerService
    {
        private readonly MySQLContext _mySQLContext;
        private readonly ComputerConverter _converter;
        public ComputerService(MySQLContext mySQLContext)
        {
            _mySQLContext = mySQLContext;
            _converter = new ComputerConverter();
        }
        public ComputerVO Create(ComputerVO computer)
        {
            try
            {
                ComputerRepository ldapGroup = new ComputerRepository(_mySQLContext);
                var computerEntity = _converter.Parse(computer);
                computerEntity = ldapGroup.Create(computerEntity);
                return _converter.Parse(computerEntity);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        public List<ComputerVO> FindAll()
        {
            ComputerRepository ldapComputer = new ComputerRepository(_mySQLContext);
            return _converter.ParseList(ldapComputer.FindAll());
        }
        public List<ComputerVO> FindByDn(string dn)
        {
            ComputerRepository ldapComputer = new ComputerRepository(_mySQLContext);
            return _converter.ParseList(ldapComputer.FindByDn(dn));
        }
        public ComputerVO FindBySamName(string dn, string samName)
        {
            ComputerRepository ldapComputer = new ComputerRepository(_mySQLContext);
            return _converter.Parse(ldapComputer.FindBySamName(dn, samName));
        }
        private bool Exist(string dn, string samName)
        {
            ComputerRepository ldapComputer = new ComputerRepository(_mySQLContext);
            if (ldapComputer.FindBySamName(dn, samName) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public ComputerVO Update(ComputerVO computer)
        {
            try
            {
                ComputerRepository ldapComputer = new ComputerRepository(_mySQLContext);
                var computerEntity = _converter.Parse(computer);
                computerEntity = ldapComputer.Update(computerEntity);
                return _converter.Parse(computerEntity);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occurred:\r\n\t" + e.GetType() + ":" + e.Message);
                return null;
            }
        }
        public bool Delete(string dn, string samName)
        {
            ComputerRepository ldapComputer = new ComputerRepository(_mySQLContext);
            CredentialRepository credential = new CredentialRepository(_mySQLContext);
            Computer result = new Computer();
            credential.DN = dn;
            result = ldapComputer.FindBySamName(credential, samName);
            try
            {
                if (result != null)
                {
                    return (ldapComputer.Delete(credential, result));
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
    }
}

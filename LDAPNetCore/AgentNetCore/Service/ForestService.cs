using AgentNetCore.Context;
using AgentNetCore.Model;
using AgentNetCore.Repository;
using System;
using System.Collections.Generic;


namespace AgentNetCore.Service
{
    public class ForestService : IForestService
    {
        private MySQLContext _mySQLContext;
        public ForestService(MySQLContext mySQLContext)
        {
            _mySQLContext = mySQLContext;
        }
        public List<Forest> FindAll()
        {
            ForestRepository ldapForest = new ForestRepository(_mySQLContext);
            return ldapForest.FindAll();
        }
        public List<Forest> FindAll(string domain)
        {
            ForestRepository ldapForest = new ForestRepository(_mySQLContext);
            return ldapForest.FindAll(domain);
        }
    }
}

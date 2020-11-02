using AgentNetCore.Context;
using AgentNetCore.Data.Converters;
using AgentNetCore.Data.VO;
using AgentNetCore.Model;
using AgentNetCore.Repository;
using System;
using System.Collections.Generic;


namespace AgentNetCore.Service
{
    public class ForestService : IForestService
    {
        private MySQLContext _mySQLContext;
        private readonly ForestConverter _converter;
        public ForestService(MySQLContext mySQLContext)
        {
            _mySQLContext = mySQLContext;
            _converter = new ForestConverter();
        }
        public List<ForestVO> FindAll()
        {
            ForestRepository ldapForest = new ForestRepository(_mySQLContext);
            return _converter.ParseList(ldapForest.FindAll());
        }
        public List<ForestVO> FindAll(string domain)
        {
            ForestRepository ldapForest = new ForestRepository(_mySQLContext);
            return _converter.ParseList(ldapForest.FindAll(domain));
        }
    }
}

﻿using AgentNetCore.Context;
using AgentNetCore.Data.Converters;
using AgentNetCore.Data.VO;
using AgentNetCore.Model;
using AgentNetCore.Repository;
using AgentNetCore.Service.Interface;
using System;
using System.Collections.Generic;


namespace AgentNetCore.Service
{
    public class ForestService : IForestService
    {
        private readonly MySQLContext _mySQLContext;
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
        public List<ForestVO> FindAll(string dn)
        {
            ForestRepository ldapForest = new ForestRepository(_mySQLContext);
            return _converter.ParseList(ldapForest.FindByDn(dn));
        }
    }
}

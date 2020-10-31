using AgentNetCore.Context;
using AgentNetCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgentNetCore.Repository
{
    public class ConfigurationRepository
    {
        MySQLContext _mySQLContext;
        public ConfigurationRepository(MySQLContext mySQLContext)
        {
            _mySQLContext = mySQLContext;
        }
        public string GetConfiguration(string field)
        {
            return (_mySQLContext.Configurations.SingleOrDefault(p => p.ConfigurationField.Equals(field))).Value;
        }
        public Configuration GetConfiguration(Configuration config)
        {
            return _mySQLContext.Configurations.SingleOrDefault(p => p.ConfigurationField.Equals(config.ConfigurationField));
        }
    }
}

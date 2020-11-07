using AgentNetCore.Context;
using AgentNetCore.Model;
using System.Linq;

namespace AgentNetCore.Repository
{
    public class ConfigurationRepository : IConfigurationRepository
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

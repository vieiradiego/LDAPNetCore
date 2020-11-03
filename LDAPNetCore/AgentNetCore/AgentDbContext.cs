using AgentNetCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace AgentNetCore
{
    public class AgentDbContext : IDesignTimeDbContextFactory<MySQLContext>
    {
        public MySQLContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<MySQLContext>();
            var connectionString = configuration["connectionStrings:MySQL"];
            builder.UseMySql(connectionString);
            return new MySQLContext(builder.Options);
        }
    }
}

using AgentNetCore.Model;
using Microsoft.EntityFrameworkCore;

namespace AgentNetCore.Context
{

    public class MySQLContext : DbContext
    {
        public MySQLContext()
        {

        }
        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }
        public DbSet<Server> Servers { get; set; }
        public DbSet<Credential> Credentials { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
    }
}


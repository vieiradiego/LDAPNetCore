using AgentNetCore.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}


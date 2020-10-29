using AgentNetCore.Model;
using Microsoft.EntityFrameworkCore;
using System.DirectoryServices.AccountManagement;

namespace AgentNetCore.Context
{

    public class LdapContext
    {
        public LdapContext()
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Computer> Computers { get; set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<Credential> Credentials { get; set; }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgentNetCore.Context
{
    public class LDAPRouter
    {
        private MySQLContext _context;
        public LDAPRouter(MySQLContext context)
        {
            _context = context;
        }

        private List <string> GetServer()
        {
            return null;
        }

        private List<string> SetServer(List<string> servers)
        {
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgentNetCore.Application
{
    public class ObjectApplication
    {
        public enum Category
        {// Object Category to LDAP
            user,
            group,
            computer,
            organizationalUnit,
            domain,
            person
        }
    }
}

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
        private string CreateContainerByCategory(ObjectApplication.Category objectCategory, string pathDomain)
        {
            string cont = "";
            string[] d = pathDomain.Split(".");
            if (objectCategory == ObjectApplication.Category.user)
            {
                cont = "cn=" + objectCategory;
                for (int i = 0; i < d.Length; i++)
                {

                    cont = cont + ",dc=" + d[i];
                }

            }
            if (objectCategory == ObjectApplication.Category.computer)
            {

            }
            if (objectCategory == ObjectApplication.Category.group)
            {
                for (int i = 0; i < d.Length; i++)
                {
                    cont = cont + "dc=" + d[i];
                    if (d.Length != (i + 1))
                    {
                        cont = cont + ",";
                    }
                }

            }
            if (objectCategory == ObjectApplication.Category.organizationalUnit)
            {

            }
            return cont;
        }
    }
}

using AgentNetCore.Repository;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

namespace AgentNetCore.Context
{
    public class DomainRepository
    {
        private readonly MySQLContext _mySQLContext;
        public DomainRepository(MySQLContext mySQLContext)
        {
            _mySQLContext = mySQLContext;
        }

        public Domain Read(Domain domain)
        {
            Forest currentForest = Forest.GetCurrentForest();
            GlobalCatalog gc = currentForest.FindGlobalCatalog();
            DirectorySearcher userSearcher = gc.GetDirectorySearcher();
            userSearcher.Filter = "(&((&(objectCategory=Person)(objectClass=User)))(samaccountname=" + domain.Name + "))";
            SearchResult result = userSearcher.FindOne();
            return null;
        }
    }
}

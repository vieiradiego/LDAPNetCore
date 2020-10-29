using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

namespace AgentNetCore.Context
{
    public class DomainRepository
    {
        public DomainRepository()
        {
            
        }

        public string GetDomain(string domain)
        {
            return "";
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

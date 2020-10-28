using AgentNetCore.Model;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;

namespace AgentNetCore.Context
{
    public class LDAPDomain
    {
        private LDAPConnect _connect;
        private DirectoryEntry _dirEntry;
        private DirectorySearcher _search;
        public LDAPDomain(Domain domain)
        {
            _connect = new LDAPConnect(domain.Name,LDAPConnect.ObjectCategory.domain);
            _dirEntry = new DirectoryEntry(_connect.Path, _connect.User, _connect.Pass);
            _search = new DirectorySearcher(_dirEntry);
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

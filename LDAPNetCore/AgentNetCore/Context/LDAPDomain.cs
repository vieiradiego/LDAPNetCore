using AgentNetCore.Model;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace AgentNetCore.Context
{
    public class LDAPDomain
    {
        private LDAPConnect _connect;
        private DirectoryEntry _dirEntry;
        private Domain _Domain;
        private DirectorySearcher _search;
        public LDAPDomain()
        {
            _connect = new LDAPConnect("domain", "marveldomain.local", "192.168.0.99", false);
            _dirEntry = new DirectoryEntry(_connect.Path, _connect.User, _connect.Pass);
            _search = new DirectorySearcher(_dirEntry);
        }
        public User Read(User domain)
        {
            return null;
        }
    }
}

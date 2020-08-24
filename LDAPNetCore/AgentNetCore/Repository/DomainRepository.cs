using Persistence.Interface;
using Persistence.Model;
using System.Collections.Generic;
using System.Threading;


namespace AgentNetCore.Business
{
    public class DomainRepository : IDomainService
    {
        private volatile int count;

        public List<Domain> FindAll()
        {
            List<Domain> domains = new List<Domain>();
            for (int i = 0; i < 8; i++)
            {
                Domain domain = MockDomains(i);
                domains.Add(domain);
            }
            return domains;
        }

        private Domain MockDomains(int i)
        {
            return new Domain
            {
                Id = IncrementAndGet(),
                Path = "" + i
            };
        }

        private long IncrementAndGet()
        {
            return Interlocked.Increment(ref count);
        }

        public Domain FindById(long id)
        {
            return new Domain
            {
                Id = 1,
                Path = ""
            };
        }
    }
}

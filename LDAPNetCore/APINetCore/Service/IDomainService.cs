using APINetCore.Model;
using System.Collections.Generic;

namespace APINetCore.Service
{
    public interface IDomainService
    {
        Domain Create(Domain domain);
        Domain Update(Domain domain);
        Domain FindById(long id);
        List<Domain> FindAll();
        void Delete(long id);
    }
}

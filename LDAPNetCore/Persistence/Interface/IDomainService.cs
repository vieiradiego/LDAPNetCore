using Persistence.Model;
using System;
using System.Collections.Generic;

namespace Persistence.Interface
{
    public interface IDomainService
    {
        Domain FindById(long id);
        List<Domain> FindAll();
    }
}

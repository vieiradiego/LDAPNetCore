using Persistence.Model;
using System.Collections.Generic;

namespace Persistence.Interface
{
    public interface IGroupService
    {
        Group Create(Group group);
        Group FindById(long id);
        List<Group> FindAll();
        Group Update(Group group);
        void Delete(long id);
    }
}

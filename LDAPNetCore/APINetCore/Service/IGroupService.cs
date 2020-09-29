using APINetCore.Model;
using System.Collections.Generic;

namespace APINetCore.Service
{
    public interface IGroupService
    {
        Group Create(Group group);
        Group Update(Group group);
        Group FindById(long id);
        List<Group> FindAll();
        void Delete(long id);
    }
}

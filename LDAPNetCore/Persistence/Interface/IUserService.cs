using Persistence.Model;
using System.Collections.Generic;

namespace Persistence.Interface
{
    public interface IUserService
    {
        // Implementar todas as ações que estão nos casos de uso do TCC
        User Create(User user);
        User FindById(long id);
        List<User> FindAll();
        User Update(User user);
        void Delete(long id);
    }
}

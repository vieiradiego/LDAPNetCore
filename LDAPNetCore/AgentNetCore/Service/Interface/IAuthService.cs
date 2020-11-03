using AgentNetCore.Data.VO;
using AgentNetCore.Model;

namespace AgentNetCore.Service
{
    public interface IAuthService
    {
        object FindByLogin(ClientVO user);
    }
}

using AgentNetCore.Data.VO;
using AgentNetCore.Model;

namespace AgentNetCore.Repository.Interface
{
    public interface IClientRepository : IRepository
    {
        Client ValidateCredentials(ClientVO client);
        Client ValidateCredentials(string secretClient);
        bool RevokeToken(string secretClient);
        Client RefreshUserInfo(Client client);
    }
}

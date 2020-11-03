using AgentNetCore.Data.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgentNetCore.Business
{
    public interface IClientBusiness
    {
        TokenVO ValidateCredentials(ClientVO user);

        TokenVO ValidateCredentials(TokenVO token);

        bool RevokeToken(string userName);
    }
}

using AgentNetCore.Data.Converter;
using AgentNetCore.Data.VO;
using AgentNetCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgentNetCore.Data.Converters
{
    public class ClientConverter : IParser<ClientVO, Client>, IParser<Client, ClientVO>
    {
        public Client Parse(ClientVO origin)
        {
            if (origin == null) return null;
            return new Client()
            {
                SecretClient = origin.UserName,
                SecretKey= origin.Password
            };
        }

        public ClientVO Parse(Client origin)
        {
            if (origin == null) return null;
            return new ClientVO()
            {
                UserName = origin.SecretClient,
                Password = origin.SecretKey
            };
        }

        public List<Client> ParseList(List<ClientVO> origin)
        {
            if (origin == null) return null;
            return origin.Select(item => Parse(item)).ToList();
        }

        public List<ClientVO> ParseList(List<Client> origin)
        {
            return origin.Select(item => Parse(item)).ToList();
        }
    }
}


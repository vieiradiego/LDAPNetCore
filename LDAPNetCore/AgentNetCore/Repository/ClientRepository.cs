using AgentNetCore.Data.VO;
using AgentNetCore.Model;
using AgentNetCore.Repository.Interface;
using AgentNetCore.Security.Configuration;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AgentNetCore.Context
{
    public class ClientRepository : IClientRepository
    {
        private readonly MySQLContext _mySQLContext;

        public ClientRepository(MySQLContext mySQLContext)
        {
            _mySQLContext = mySQLContext;
        }

        public Client ValidateCredentials(ClientVO client)
        {
            var pass = ComputeHash(client.Password, new SHA256CryptoServiceProvider());
            //Client cC = new Client();
            //cC.FirstName = "Admin";
            //cC.SurName= "istrator";
            //cC.SecretClient = client.UserName;
            //cC.SecretKey = pass;
            //_mySQLContext.Clients.Update(cC);
            return _mySQLContext.Clients.FirstOrDefault(c=>((c.SecretClient == client.UserName) && (c.SecretKey == pass)));
        }

        public Client ValidateCredentials(string userName)
        {
            return _mySQLContext.Clients.SingleOrDefault(u => (u.SecretClient == userName));
        }

        public bool RevokeToken(string userName)
        {
            var client = _mySQLContext.Clients.SingleOrDefault(u => (u.SecretClient == userName));
            if (client is null) return false;
            client.RefreshToken = null;
            _mySQLContext.SaveChanges();
            return true;
        }

        public Client RefreshUserInfo(Client client)
        {
            if (!_mySQLContext.Clients.Any(u => u.Id.Equals(client.Id))) return null;

            var result = _mySQLContext.Clients.SingleOrDefault(p => p.Id.Equals(client.Id));
            if (result != null)
            {
                try
                {
                    _mySQLContext.Entry(result).CurrentValues.SetValues(client);
                    _mySQLContext.SaveChanges();
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return result;
        }

        private string ComputeHash(string input, SHA256CryptoServiceProvider algorithm)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);
            return BitConverter.ToString(hashedBytes);
        }

    }
}


using AgentNetCore.Context;
using AgentNetCore.Data.VO;
using AgentNetCore.Model;
using AgentNetCore.Repository.Interface;
using AgentNetCore.Security.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace AgentNetCore.Service
{
    public class AuthService : IAuthService
    {
        private IClientRepository _service;
        private SigningConfiguration _signingConfigurations;
        private TokenConfiguration _tokenConfigurations;
        public AuthService(IClientRepository service, SigningConfiguration signingConfiguration, TokenConfiguration tokenConfiguration)
        {
            _service = service;
            _signingConfigurations = signingConfiguration;
            _tokenConfigurations = tokenConfiguration;
        }

        public object FindByLogin(ClientVO user)
        {
            bool credentialsIsValid = false;
            if (user != null && !string.IsNullOrWhiteSpace(user.UserName))
            {
                var baseUser = _service.ValidateCredentials(user.UserName);
                credentialsIsValid = (baseUser != null && user.UserName == baseUser.SecretClient && user.Password == baseUser.SecretKey);
            }
            if (credentialsIsValid)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(user.UserName, "Login"),
                        new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                        }
                    );

                DateTime createDate = DateTime.Now;
                DateTime expirationDate = createDate + TimeSpan.FromMinutes(_tokenConfigurations.Minutes);

                var handler = new JwtSecurityTokenHandler();
                string token = CreateToken(identity, createDate, expirationDate, handler);

                return SuccessObject(createDate, expirationDate, token);
            }
            else
            {
                return ExceptionObject();
            }
        }

        private string CreateToken(ClaimsIdentity identity, DateTime createDate, DateTime expirationDate, JwtSecurityTokenHandler handler)
        {
            var securityToken = handler.CreateToken(new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Issuer = _tokenConfigurations.Issuer,
                Audience = _tokenConfigurations.Audience,
                SigningCredentials = _signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = createDate,
                Expires = expirationDate
            });

            var token = handler.WriteToken(securityToken);
            return token;
        }

        private object ExceptionObject()
        {
            return new
            {
                autenticated = false,
                message = "Failed to autheticate"
            };
        }

        private object SuccessObject(DateTime createDate, DateTime expirationDate, string token)
        {
            return new
            {
                autenticated = true,
                created = createDate.ToString("yyyy-MM-dd HH:mm:ss"),
                expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                accessToken = token,
                message = "OK"
            };
        }
    }
}

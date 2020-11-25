using AgentNetCore.Business;
using AgentNetCore.Data.VO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace AgentNetCore.Controllers
{

    [ApiVersion("1.0")]
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private IClientBusiness _loginBusiness;
        private readonly ILogger _logger;

        public AuthController(IClientBusiness loginBusiness)
        {
            _loginBusiness = loginBusiness;
        }
        /// <summary>
        /// AUTENTICAR um Cliente.
        /// </summary>
        /// <remarks>
        /// Retorna um objeto no formato TokenVO
        /// </remarks>
        /// <returns>O retorno desse serviço é um determinado TokenVO 
        /// com os dados necessários para acessar a API REST</returns>
        /// <param name="client"></param>
        [HttpPost]
        [SwaggerResponse((200), Type = typeof(TokenVO))]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [SwaggerResponse(405)]
        [Route("signin")]
        public IActionResult Signin([FromBody] ClientVO client)
        {
            if (client == null) return BadRequest("Invalid client request");
            if (!string.IsNullOrWhiteSpace(client.UserName) && !string.IsNullOrWhiteSpace(client.Password))
            {
                var token = _loginBusiness.ValidateCredentials(client);
                if (token == null) return Unauthorized("Unauthorized client request");
                return Ok(token);
            }
            else
            {
                return BadRequest("Invalid client request");
            }
        }
        /// <summary>
        /// ATUALIZAR os dados de acesso de um Cliente.
        /// </summary>
        /// <remarks>
        /// Retorna um objeto no formato TokenVO
        /// </remarks>
        /// <returns>O retorno desse serviço é um determinado TokenVO com os dados necessários para acessar a API REST</returns>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        [HttpPost]
        [Route("refresh")]
        [SwaggerResponse((200), Type = typeof(TokenVO))]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [SwaggerResponse(405)]
        public IActionResult Refresh([FromBody] TokenVO token)
        {
            if (token is null) return BadRequest("Invalid client request");
            if (!string.IsNullOrWhiteSpace(token.AccessToken) && !string.IsNullOrWhiteSpace(token.RefreshToken))
            {
                var tokenVar = _loginBusiness.ValidateCredentials(token);
                if (tokenVar == null) return Unauthorized("Unauthorized client request");
                return Ok(tokenVar);
            }
            else
            {
                return BadRequest("Invalid client request");
            }
        }
        /// <summary>
        /// ATUALIZAR os dados de acesso de um Cliente.
        /// </summary>
        /// <remarks>
        /// Não há retorno de objetos nesse método
        /// </remarks>
        /// <returns>O retorno desse serviço é código HTTP</returns>
        /// <param name="userName"></param>
        [HttpPost]
        [Route("revoke")]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(405)]
        [Authorize("Bearer")]
        public IActionResult Revoke()
        {
            var username = User.Identity.Name;
            var result = _loginBusiness.RevokeToken(username);
            if (!result) return BadRequest("Invalid client request");
            return NoContent();
        }
    }
}

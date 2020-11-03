using AgentNetCore.Business;
using AgentNetCore.Data.VO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgentNetCore.Controllers
{

    [ApiVersion("1.0")]
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private IClientBusiness _loginBusiness;

        public AuthController(IClientBusiness loginBusiness)
        {
            _loginBusiness = loginBusiness;
        }

        /// <summary>
        /// Autenticação do Client da API Agent.
        /// </summary>
        /// <param name="client"></param>
        /// <returns>Retorna os campos do Client</returns>
        [HttpPost]
        [Route("signin")]
        public IActionResult Signin([FromBody] ClientVO client)
        {
            if (client == null) return BadRequest("Ivalid client request");
            var token = _loginBusiness.ValidateCredentials(client);
            if (token == null) return Unauthorized();
            return Ok(token);
        }
        /// <summary>
        /// Renovação do Token para o Client da API Agent
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Retorna os campos do Token</returns>
        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh([FromBody] TokenVO token)
        {
            if (token is null) return BadRequest("Ivalid client request");
            var tokenVar = _loginBusiness.ValidateCredentials(token);
            if (tokenVar == null) return BadRequest("Ivalid client request");
            return Ok(tokenVar);
        }

        /// <summary>
        /// Revogação do Token para o Client da API Agent
        /// </summary>
        /// <returns>Retorno No Content</returns>
        [HttpGet]
        [Route("revoke")]
        [Authorize("Bearer")]
        public IActionResult Revoke()
        {
            var username = User.Identity.Name;
            var result = _loginBusiness.RevokeToken(username);
            if (!result) return BadRequest("Ivalid client request");
            return NoContent();
        }
    }
}

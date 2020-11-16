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
        /// AUTENTICAR um Cliente.
        /// </summary>
        /// <remarks>
        /// Retorna um objeto no formato TokenVO
        /// </remarks>
        /// <returns>O retorno desse serviço é um determinado TokenVO 
        /// com os dados necessários para acessar a API REST</returns>
        /// <param name="client"></param>
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
        /// ATUALIZAR os dados de acesso de um Cliente.
        /// </summary>
        /// <remarks>
        /// Retorna um objeto no formato TokenVO
        /// </remarks>
        /// <returns>O retorno desse serviço é um determinado TokenVO com os dados necessários para acessar a API REST</returns>
        /// <param name="token"></param>
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
        /// ATUALIZAR os dados de acesso de um Cliente.
        /// </summary>
        /// <remarks>
        /// Não há retorno de objetos nesse método
        /// </remarks>
        /// <returns>O retorno desse serviço é código HTTP</returns>
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

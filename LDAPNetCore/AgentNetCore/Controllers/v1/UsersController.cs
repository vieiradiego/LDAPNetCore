using AgentNetCore.Data.VO;
using AgentNetCore.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using Tapioca.HATEOAS;

namespace AgentNetCore.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class UsersController : ControllerBase, IController
    {
        private IUserService _userService;
        private readonly ILogger _logger;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// RECUPERAR os dados de todos os Usuários. 
        /// </summary>
        /// <remarks>
        /// Retorna uma lista de objetos no formato UserVO
        /// </remarks>
        /// <returns>O retorno desse serviço é uma lista de UserVO </returns>
        [HttpGet]
        [SwaggerResponse((200), Type = typeof(List<UserVO>))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get()
        {
            return Ok(_userService.FindAll());
        }

        /// <summary>
        /// RECUPERAR os dados de um determinado Usuário
        /// </summary>
        /// <remarks>
        /// Retorna um objeto no formato UsuarioVO
        /// </remarks>
        /// <returns>O retorno desse serviço é um determinado UsuarioVO encontrado</returns>
        /// <param name="dn"></param>
        /// <param name="email"></param>
        /// <param name="samName"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        [HttpGet("find")]
        [SwaggerResponse((200), Type = typeof(UserVO))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [SwaggerResponse(404)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Find([FromQuery] string dn,      [FromQuery] string email, [FromQuery] string samName,
                                  [FromQuery] string firstName, [FromQuery] string lastName)
        {
            
            if (!string.IsNullOrWhiteSpace(dn) && !string.IsNullOrWhiteSpace(email))
            {
                var user = _userService.FindByEmail(dn, email);
                if (user == null) return NotFound();
                return Ok(user);
            }
            else if (!string.IsNullOrWhiteSpace(dn) && !string.IsNullOrWhiteSpace(samName))
            {
                var user = _userService.FindBySamName(dn, samName);
                if (user == null) return NotFound();
                return Ok(user);
            }
            else if (!string.IsNullOrWhiteSpace(dn) && !string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName))
            {
                var user = _userService.FindByName(dn, firstName, lastName);
                if (user == null) return NotFound();
                return Ok(user);
            }
            else if (!string.IsNullOrWhiteSpace(dn) && !string.IsNullOrWhiteSpace(firstName))
            {
                var user = _userService.FindByFirstName(dn, firstName);
                if (user == null) return NotFound();
                return Ok(user);
            }
            else if (!string.IsNullOrWhiteSpace(dn) && !string.IsNullOrWhiteSpace(lastName))
            {
                var user = _userService.FindByLastName(dn, lastName);
                if (user == null) return NotFound();
                return Ok(user);
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// CRIAR um Usuário
        /// </summary>
        /// <remarks>
        /// Retorna um objeto no formato UserVO
        /// </remarks>
        /// <returns>O retorno desse serviço é um UserVO criado</returns>
        /// <param name="user"></param>
        [HttpPost]
        [SwaggerResponse((201), Type = typeof(UserVO))]
        [SwaggerResponse(209)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Post([FromBody] UserVO user)
        {
            if (user == null) return BadRequest();
            var newUser = new ObjectResult(_userService.Create(user));
            if (newUser.Value == null) return Conflict();
            return newUser;
        }

        /// <summary>
        /// ATUALIZAR um Usuário
        /// </summary>
        /// <remarks>
        /// Retorna um objeto no formato UserVO
        /// </remarks>
        /// <returns>O retorno desse serviço é um UserVO atualizado</returns>
        /// <param name="user"></param>
        [HttpPut]
        [SwaggerResponse((202), Type = typeof(UserVO))]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Put([FromBody] UserVO user)
        {
            if (user == null) return BadRequest();
            return new ObjectResult(_userService.Update(user));
        }

        /// <summary>
        /// DELETAR um Usuário
        /// </summary>
        /// <remarks>
        /// Não há retorno de objetos nesse método
        /// </remarks>
        /// <returns>O retorno desse serviço é código HTTP</returns>
        /// <param name="dn"></param>
        /// <param name="samname"></param>
        [HttpDelete]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Delete([FromQuery] string dn, [FromQuery] string samname)
        {
            if (!string.IsNullOrWhiteSpace(dn) && !string.IsNullOrWhiteSpace(samname))
            {
                _userService.Delete(dn, samname);
                return NoContent();
            }
            return BadRequest();
        }
        /// <summary>
        /// RECUPERAR os Grupos de um Usuário
        /// </summary>
        /// <remarks>
        /// Retorna uma lista de objetos no formato GrupoVO
        /// </remarks>
        /// <returns>O retorno desse serviço é uma lista de GrupoVO encontrados pelo Usuário informado</returns>
        /// <param name="samNameUser"></param>
        [HttpGet("groups")]
        [SwaggerResponse((200), Type = typeof(List<GroupVO>))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [SwaggerResponse(404)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetGroups(string samNameUser)
        {
            //_userService.Delete(domain, email);
            return NoContent();
        }
    }
}

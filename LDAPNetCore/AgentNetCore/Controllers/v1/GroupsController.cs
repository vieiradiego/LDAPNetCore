using AgentNetCore.Data.VO;
using AgentNetCore.Model;
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
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class GroupsController : ControllerBase, IController
    {
        private IGroupService _groupService;
        private readonly ILogger _logger;
        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        /// <summary>
        /// RECUPERAR todos os Grupos
        /// </summary>
        /// <remarks>
        /// Retorna uma lista de objetos no formato GroupVO
        /// </remarks>
        /// <returns>O retorno desse serviço é uma lista de GroupVO </returns>
        [HttpGet]
        [SwaggerResponse((200), Type = typeof(List<GroupVO>))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get()
        {
            var group = _groupService.FindAll();
            if (group == null) return NotFound();
            return Ok(group);
        }

        /// <summary>
        /// RECUPERAR os dados para um determinado Grupo
        /// </summary>
        /// <remarks>
        /// Retorna um objeto no formato GroupVO
        /// </remarks>
        /// <returns>O retorno desse serviço é um determinado GroupVO encontrado</returns>
        /// <param name="domain"></param>
        /// <param name="samName"></param>
        [HttpGet("domain/samName")]
        [SwaggerResponse((200), Type = typeof(GroupVO))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetBySamName(string domain, string samName)
        {
            var group = this._groupService.FindBySamName(domain, samName);
            if (group == null) return NotFound();
            return Ok(group);
        }

        /// <summary>
        /// CRIAR um Grupo
        /// </summary>
        /// <remarks>
        /// Retorna um objeto no formato GroupVO
        /// </remarks>
        /// <returns>O retorno desse serviço é um GroupVO criado</returns>
        /// <param name="group"></param>
        [HttpPost]
        [SwaggerResponse((201), Type = typeof(GroupVO))]
        [SwaggerResponse(209)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Post([FromBody] GroupVO group)
        {
            if (group == null) return BadRequest();
            var newGroup = new ObjectResult(this._groupService.Create(group));
            if (newGroup.Value == null) return Conflict();
            return newGroup;
        }

        /// <summary>
        /// ATUALIZAR um Grupo
        /// </summary>
        /// <remarks>
        /// Retorna um objeto no formato GroupVO
        /// </remarks>
        /// <returns>O retorno desse serviço é um GroupVO atualizado</returns>
        /// <param name="group"></param>
        [HttpPut]
        [SwaggerResponse((202), Type = typeof(GroupVO))]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Put([FromBody] GroupVO group)
        {
            if (group == null) return BadRequest();
            return new ObjectResult(this._groupService.Update(group));
        }

        /// <summary>
        /// DELETAR um Grupo
        /// </summary>
        /// <remarks>
        /// Não há retorno de objetos nesse método
        /// </remarks>
        /// <returns>O retorno desse serviço é código HTTP</returns>
        /// <param name="domain"></param>
        /// <param name="samName"></param>
        [HttpDelete("domain/samName")]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Delete(string domain, string samName)
        {
            this._groupService.Delete(domain, samName);
            return NoContent();
        }
        /// <summary>
        /// RECUPERAR os Usuários de um Grupo
        /// </summary>
        /// <remarks>
        /// Retorna uma lista de objetos no formato UserVO
        /// </remarks>
        /// <returns>O retorno desse serviço é uma lista de UserVO encontrados pelo Usuário informado</returns>
        /// <param name="group"></param>
        [HttpGet("users")]
        [SwaggerResponse((200), Type = typeof(List<UserVO>))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [SwaggerResponse(404)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetUsers([FromBody] GroupVO group)
        {
            //_userService.Delete(domain, email);
            return NoContent();
        }
    }
}

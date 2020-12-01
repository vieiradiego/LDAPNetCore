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
        public IActionResult Get([FromQuery] string dn)
        {
            if (!string.IsNullOrWhiteSpace(dn))
            {
                var user = _groupService.FindByDn(dn);
                if (user == null) return NotFound();
                return Ok(user);
            }
            else
            {
                return Ok(_groupService.FindAll());
            }
        }

        /// <summary>
        /// RECUPERAR os dados para um determinado Grupo
        /// </summary>
        /// <remarks>
        /// Retorna um objeto no formato GroupVO
        /// </remarks>
        /// <returns>O retorno desse serviço é um determinado GroupVO encontrado</returns>
        /// <param name="dn"></param>
        /// <param name="email"></param>
        /// <param name="samName"></param>
        [HttpGet("find")]
        [SwaggerResponse((200), Type = typeof(GroupVO))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [SwaggerResponse(404)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetBySamName([FromQuery] string dn, [FromQuery] string email,
                                          [FromQuery] string samName)
        {
            if (!string.IsNullOrWhiteSpace(dn) && !string.IsNullOrWhiteSpace(email))
            {
                var group = _groupService.FindByEmail(dn, email);
                if (group == null) return NotFound();
                return Ok(group);
            }
            else if (!string.IsNullOrWhiteSpace(dn) && !string.IsNullOrWhiteSpace(samName))
            {
                var group = _groupService.FindBySamName(dn, samName);
                if (group == null) return NotFound();
                return Ok(group);
            }
            else
            {
                return BadRequest();
            }
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
        /// <param name="dn"></param>
        /// <param name="samName"></param>
        [HttpDelete]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Delete([FromQuery] string dn, [FromQuery] string samName)
        {
            if (!string.IsNullOrWhiteSpace(dn) && !string.IsNullOrWhiteSpace(samName))
            {
                if(this._groupService.Delete(dn, samName)) return NoContent(); ;
                return Conflict();
            }
            return BadRequest();
        }
        /// <summary>
        /// ADICIONAR um Usuário em um Grupo
        /// </summary>
        /// <remarks>
        /// Não há retorno de objetos nesse método
        /// </remarks>
        /// <returns>O retorno desse serviço é código HTTP</returns>
        /// <param name="userDn"></param>
        /// <param name="groupDn"></param>
        [HttpPost("adduser")]
        [SwaggerResponse(200)]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [SwaggerResponse(404)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult AddUser([FromQuery] string userDn, [FromQuery] string groupDn)
        {
            if (!string.IsNullOrWhiteSpace(userDn) && !string.IsNullOrWhiteSpace(groupDn))
            {
                if (this._groupService.AddUser(userDn, groupDn)) return Ok();
                return Conflict();
            }
            return BadRequest();
        }
        /// <summary>
        /// REMOVER um Usuário em um Grupo
        /// </summary>
        /// <remarks>
        /// Não há retorno de objetos nesse método
        /// </remarks>
        /// <returns>O retorno desse serviço é código HTTP</returns>
        /// <param name="userDn"></param>
        /// <param name="groupDn"></param>
        [HttpPost("removeuser")]
        [SwaggerResponse(200)]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [SwaggerResponse(404)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult RemoveUser([FromQuery] string userDn, [FromQuery] string groupDn)
        {
            if (!string.IsNullOrWhiteSpace(userDn) && !string.IsNullOrWhiteSpace(groupDn))
            {
                if (this._groupService.RemoveUser(userDn, groupDn)) return Ok();
                return Conflict();
            }
            return BadRequest();
        }
        /// <summary>
        /// ALTERAR o Usuário de um Grupo Antigo para um Grupo Novo
        /// </summary>
        /// <remarks>
        /// Retorna uma lista de objetos no formato GroupVO
        /// </remarks>
        /// <returns>O retorno desse serviço é uma lista de GroupVO encontrados no Usuário informado</returns>
        /// <param name="userDn"></param>
        /// <param name="newGroupDn"></param>
        /// <param name="oldGroupDn"></param>
        [HttpPost("changegroup")]
        [SwaggerResponse(200)]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [SwaggerResponse(404)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult ChangeGroup([FromQuery] string userDn, [FromQuery] string newGroupDn, [FromQuery] string oldGroupDn)
        {
            if ((!string.IsNullOrWhiteSpace(userDn)) && (!string.IsNullOrWhiteSpace(newGroupDn))&& (!string.IsNullOrWhiteSpace(oldGroupDn)))
            {
                this._groupService.ChangeGroup(userDn, newGroupDn, oldGroupDn);
                return Ok();
            }
            return BadRequest();
        }
        /// <summary>
        /// RECUPERAR os Grupos de um Usuário
        /// </summary>
        /// <remarks>
        /// Retorna uma lista de objetos no formato GroupVO
        /// </remarks>
        /// <returns>O retorno desse serviço é uma lista de GroupVO encontrados pelo Usuário informado</returns>
        /// <param name="userDn"></param>
        [HttpGet("groupsbyuser")]
        [SwaggerResponse((200), Type = typeof(List<GroupVO>))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [SwaggerResponse(404)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetGroups([FromQuery] string userDn)
        {
            if (!string.IsNullOrWhiteSpace(userDn))
            {
                return new ObjectResult(_groupService.GetGroups(userDn));
            }
            return BadRequest();
        }
    }
}

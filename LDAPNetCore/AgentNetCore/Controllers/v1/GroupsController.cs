﻿using AgentNetCore.Data.VO;
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
                this._groupService.Delete(dn, samName);
                return NoContent();
            }
            return BadRequest();
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
        /// <summary>
        /// INCLUIR um Usuário em um Grupo
        /// </summary>
        /// <remarks>
        /// Retorna uma lista de objetos no formato UserVO
        /// </remarks>
        /// <returns>O retorno desse serviço é uma lista de UserVO encontrados pelo Usuário informado</returns>
        /// <param name="samNameUser"></param>
        /// <param name="samNameNewGroup"></param>
        [HttpPost("adduser")]
        [SwaggerResponse((200), Type = typeof(List<UserVO>))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [SwaggerResponse(404)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult AddUser([FromQuery] string samNameUser, [FromQuery] string samNameNewGroup)
        {
            //_userService.Delete(domain, email);
            return NoContent();
        }
        /// <summary>
        /// ALTERAR o Usuário de um Grupo Antigo para um Grupo Novo
        /// </summary>
        /// <remarks>
        /// Retorna uma lista de objetos no formato GroupVO
        /// </remarks>
        /// <returns>O retorno desse serviço é uma lista de GroupVO encontrados no Usuário informado</returns>
        /// <param name="samNameUser"></param>
        /// <param name="samNameNewGroup"></param>
        /// <param name="samNameOldGroup"></param>
        [HttpPost("changegroup")]
        [SwaggerResponse((200), Type = typeof(List<GroupVO>))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [SwaggerResponse(404)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult ChangeGroup([FromQuery] string samNameUser, [FromQuery] string samNameNewGroup, [FromQuery] string samNameOldGroup)
        {
            return NoContent();
        }
    }
}

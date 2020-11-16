﻿using AgentNetCore.Data.VO;
using AgentNetCore.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using Tapioca.HATEOAS;

namespace AgentNetCore.Controllers
{
    //_logger.LogInformation(this.ControllerContext.RouteData.Values["controller"].ToString() + "|" +
    //                       this.ControllerContext.RouteData.Values["action"].ToString() + "|" +
    //                       this.ControllerContext.RouteData.Values["version"].ToString() + "|" +
    //                       Request.Host + "|" +
    //                       System.DateTime.Now.ToString("dd-MMM-yyyy-HH:mm:ss")
    //                       );
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class UsersController : ControllerBase, IController
    {
        private IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// RECUPERAR todos os Usuários
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
        /// RECUPERAR os dados para um determinado Usuário
        /// </summary>
        /// <remarks>
        /// Retorna um objeto no formato UserVO
        /// </remarks>
        /// <returns>O retorno desse serviço é um determinado UserVO encontrado</returns>
        /// <param name="email"></param>
        [HttpGet("email")]
        [SwaggerResponse((200), Type = typeof(UserVO))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [SwaggerResponse(404)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetByEmail([FromQuery] string email)
        {
            var user = _userService.FindByEmail(email);
            if (user == null) return NotFound();
            return Ok(user);
        }
        /// <summary>
        /// RECUPERAR os dados de um determinado Usuário
        /// </summary>
        /// <remarks>
        /// Retorna um objeto no formato UsuarioVO
        /// </remarks>
        /// <returns>O retorno desse serviço é um determinado UsuarioVO encontrado</returns>
        /// <param name="domain"></param>
        /// <param name="samName"></param>
        [HttpGet("domain/samName")]
        [SwaggerResponse((200), Type = typeof(UserVO))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [SwaggerResponse(404)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetBySamName([FromQuery] string domain, [FromQuery] string samName)
        {
            var user = _userService.FindBySamName(domain, samName);
            if (user == null) return NotFound();
            return Ok(user);
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
        /// <param name="domain"></param>
        /// <param name="email"></param>
        [HttpDelete("domain/samName")]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Delete([FromQuery]string domain, [FromQuery]string email)
        {
            _userService.Delete(domain, email);
            return NoContent();
        }
        /// <summary>
        /// RECUPERAR os Grupos de um Usuário
        /// </summary>
        /// <remarks>
        /// Retorna uma lista de objetos no formato GrupoVO
        /// </remarks>
        /// <returns>O retorno desse serviço é uma lista de GrupoVO encontrados pelo Usuário informado</returns>
        /// <param name="user"></param>
        [HttpGet("groups")]
        [SwaggerResponse((200), Type = typeof(List<GroupVO>))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [SwaggerResponse(404)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetGroups([FromBody] UserVO user)
        {
            //_userService.Delete(domain, email);
            return NoContent();
        }
    }
}

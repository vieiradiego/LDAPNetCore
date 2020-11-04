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
    public class GroupsController : ControllerBase
    {
        private IGroupService _groupService;
        private readonly ILogger _logger;
        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }
        /// <summary>
        /// GET para todos os grupos de segurança dos diretórios disponíveis
        /// </summary>
        /// <remarks>
        /// Retorna todos os objetos no formato GrupoVo
        /// </remarks>
        /// <returns>O retorno desse serviço é uma List<GroupVO></returns>
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
        /// GET para um determinado grupo de segurança dos diretórios disponíveis
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="samName"></param>
        /// <returns></returns>
        [HttpGet("{domain}/{samName}")]
        [SwaggerResponse((200), Type = typeof(GroupVO))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get(string domain, string samName)
        {
            var group = this._groupService.FindBySamName(domain, samName);
            if (group == null) return NotFound();
            return Ok(group);
        }

        /// <summary>
        /// POST para um determinado grupo dos diretórios disponíveis
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
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
        /// PUT para um determinado grupo dos diretórios disponíveis
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
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
        /// DELETE um determinado grupos dos diretórios disponíveis
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="samName"></param>
        /// <returns></returns>
        [HttpDelete("{domain}/{samName}")]
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
    }
}

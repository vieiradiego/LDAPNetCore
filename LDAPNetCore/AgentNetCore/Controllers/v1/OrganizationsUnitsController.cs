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
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class OrganizationsUnitsController : ControllerBase, IController
    {
        private IOrganizationalUnitService _orgService;
        private readonly ILogger _logger;
        public OrganizationsUnitsController(IOrganizationalUnitService orgService, ILogger<UsersController> logger)
        {
            _orgService = orgService;
            _logger = logger;
        }
        /// <summary>
        /// RECUPERAR todas as Unidades Organizacionais (OU)
        /// </summary>
        /// <remarks>
        /// Retorna uma lista de objetos no formato OrganizationalUnitVO
        /// </remarks>
        /// <returns>O retorno desse serviço é uma lista de OrganizationalUnitVO </returns>
        /// <param name="domain"></param>
        [HttpGet("domain")]
        [SwaggerResponse((200), Type = typeof(List<OrganizationalUnitVO>))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get(string domain)
        {
            var orgUnit = _orgService.FindAll(domain);
            if (orgUnit == null) return NotFound();
            return Ok(orgUnit);
        }

        /// <summary>
        /// RECUPERAR os dados de uma determinada Unidade Organizacional (OU)
        /// </summary>
        /// <remarks>
        /// Retorna um objeto no formato OrganizationalUnitVO
        /// </remarks>
        /// <returns>O retorno desse serviço é um determinado OrganizationalUnitVO encontrado</returns>
        /// <param name="domain"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("domain/name")]
        [SwaggerResponse((200), Type = typeof(OrganizationalUnitVO))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [SwaggerResponse(404)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get([FromQuery] string domain, [FromQuery] string name)
        {
            var orgUnit = _orgService.FindByName(domain, name);
            if (orgUnit == null) return NotFound();
            return Ok(orgUnit);
        }

        /// <summary>
        /// CRIAR uma Unidade Organizacional (OU)
        /// </summary>
        /// <remarks>
        /// Retorna um objeto no formato OrganizationalUnitVO
        /// </remarks>
        /// <returns>O retorno desse serviço é um OrganizationalUnitVO criado</returns>
        /// <param name="orgUnit"></param>
        [HttpPost]
        [SwaggerResponse((201), Type = typeof(OrganizationalUnitVO))]
        [SwaggerResponse(209)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Post([FromBody] OrganizationalUnitVO orgUnit)
        {
            if (orgUnit == null) return BadRequest();
            var newOrgUnit = new ObjectResult(_orgService.Create(orgUnit));
            if (newOrgUnit.Value == null) return Conflict();
            return newOrgUnit;
        }

        /// <summary>
        /// ATUALIZAR uma Unidade Organizacional (OU)
        /// </summary>
        /// <remarks>
        /// Retorna um objeto no formato OrganizationalUnitVO
        /// </remarks>
        /// <returns>O retorno desse serviço é um OrganizationalUnitVO atualizado</returns>
        /// <param name="orgUnit"></param>
        [HttpPut]
        [SwaggerResponse((202), Type = typeof(UserVO))]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Put([FromBody] OrganizationalUnitVO orgUnit)
        {
            if (orgUnit == null) return BadRequest();
            return new ObjectResult(_orgService.Update(orgUnit));
        }

        /// <summary>
        /// DELETAR uma Unidade Organizacional (OU)
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
        public IActionResult Delete([FromQuery] string domain, [FromQuery] string samName)
        {
            this._orgService.Delete(domain, samName);
            return NoContent();
        }
    }
}

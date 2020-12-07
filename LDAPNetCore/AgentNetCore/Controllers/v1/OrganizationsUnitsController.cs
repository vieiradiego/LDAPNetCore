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
        /// <param name="dn"></param>
        [HttpGet]
        [SwaggerResponse((200), Type = typeof(List<OrganizationalUnitVO>))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get([FromQuery] string dn)
        {
            if (!string.IsNullOrWhiteSpace(dn))
            {
                var orgUnit = _orgService.FindByDn(dn);
                if (orgUnit == null) return NotFound();
                return Ok(orgUnit);
            }
            else
            {
                return Ok(_orgService.FindAll());
            }
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
        [HttpGet("find")]
        [SwaggerResponse((200), Type = typeof(OrganizationalUnitVO))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [SwaggerResponse(404)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Find([FromQuery] string dn, [FromQuery] string name, [FromQuery] string ou)
        {
            if (!string.IsNullOrWhiteSpace(dn) && !string.IsNullOrWhiteSpace(name))
            {
                var orgUnit = _orgService.FindByName(dn, name);
                if (orgUnit == null) return NotFound();
                return Ok(orgUnit);
            }
            if (!string.IsNullOrWhiteSpace(dn) && !string.IsNullOrWhiteSpace(ou))
            {
                var orgUnit = _orgService.FindByOu(dn, ou);
                if (orgUnit == null) return NotFound();
                return Ok(orgUnit);
            }
            else
            {
                return BadRequest();
            }
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
        [SwaggerResponse((202), Type = typeof(OrganizationalUnitVO))]
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
        /// <param name="dn"></param>
        /// <param name="name"></param>
        [HttpDelete]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Delete([FromQuery] string dn, 
                                    [FromQuery] string name)
        {
            if (!string.IsNullOrWhiteSpace(dn) && !string.IsNullOrWhiteSpace(name))
            {
                if (_orgService.Delete(dn, name)) return Ok();
                return NoContent();
            }
            return BadRequest();
        }
    }
}

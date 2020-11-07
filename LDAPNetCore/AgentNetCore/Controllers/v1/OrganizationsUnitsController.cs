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
        //GET api/organizationunits/domain
        [HttpGet("{domain}")]
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

        //GET api/organizationunit/domain/samName
        [HttpGet("{domain}/{nameOU}")]
        [SwaggerResponse((200), Type = typeof(OrganizationalUnitVO))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [SwaggerResponse(404)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get(string domain, string nameOU)
        {
            var orgUnit = _orgService.FindByName(domain, nameOU);
            if (orgUnit == null) return NotFound();
            return Ok(orgUnit);
        }

        // POST api/organizationunit/orgUnit
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

        // PUT api/organizationunit/orgUnit
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

        // DELETE api/organizationunit/domain/samName/
        [HttpDelete("{domain}/{samName}")]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Delete(string domain, string samName)
        {
            this._orgService.Delete(domain, samName);
            return NoContent();
        }
    }
}

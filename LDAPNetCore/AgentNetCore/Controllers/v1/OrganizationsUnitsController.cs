using AgentNetCore.Data.VO;
using AgentNetCore.Model;
using AgentNetCore.Service;
using Microsoft.AspNetCore.Mvc;

namespace AgentNetCore.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class OrganizationsUnitsController : ControllerBase
    {
        private IOrganizationalUnitService _orgService;
        public OrganizationsUnitsController(IOrganizationalUnitService orgService)
        {
            _orgService = orgService;
        }
        //GET api/organizationunits/domain
        [HttpGet("{domain}")]
        public IActionResult Get(string domain)
        {
            var orgUnit = _orgService.FindAll(domain);
            if (orgUnit == null) return NotFound();
            return Ok(orgUnit);
        }

        //GET api/organizationunit/domain/samName
        [HttpGet("{domain}/{nameOU}")]
        public IActionResult Get(string domain, string nameOU)
        {
            var orgUnit = _orgService.FindByName(domain, nameOU);
            if (orgUnit == null) return NotFound();
            return Ok(orgUnit);
        }

        // POST api/organizationunit/orgUnit
        [HttpPost]
        public IActionResult Post([FromBody] OrganizationalUnitVO orgUnit)
        {
            if (orgUnit == null) return BadRequest();
            var newOrgUnit = new ObjectResult(_orgService.Create(orgUnit));
            if (newOrgUnit.Value == null) return Conflict();
            return newOrgUnit;
        }

        // PUT api/organizationunit/orgUnit
        [HttpPut]
        public IActionResult Put([FromBody] OrganizationalUnitVO orgUnit)
        {
            if (orgUnit == null) return BadRequest();
            return new ObjectResult(_orgService.Update(orgUnit));
        }

        // DELETE api/organizationunit/domain/samName/
        [HttpDelete("{domain}/{samName}")]
        public IActionResult Delete(string domain, string samName)
        {
            this._orgService.Delete(domain, samName);
            return NoContent();
        }
    }
}

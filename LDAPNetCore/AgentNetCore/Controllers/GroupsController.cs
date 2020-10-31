using AgentNetCore.Model;
using AgentNetCore.Service;
using Microsoft.AspNetCore.Mvc;

namespace AgentNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GroupsController : ControllerBase
    {
        private IGroupService groupService;
        public GroupsController(IGroupService groupService)
        {
            this.groupService = groupService;
        }
        //GET api/group/domain
        [HttpGet("{domain}")]
        public IActionResult Get(string domain)
        {
            return Ok(this.groupService.FindAll(domain));
        }

        //GET api/group/samName
        [HttpGet("{domain,samName}")]
        public IActionResult Get(string domain, string samName)
        {
            var group = this.groupService.FindBySamName(domain, samName);
            if (group == null) return NotFound();
            return Ok(group);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] Group group)
        {
            if (group == null) return BadRequest();
            return new ObjectResult(this.groupService.Create(group));
        }

        // PUT api/values
        [HttpPut]
        public IActionResult Put([FromBody] Group group)
        {
            if (group == null) return BadRequest();
            return new ObjectResult(this.groupService.Update(group));
        }

        // DELETE api/domain/samName/
        [HttpDelete("{domain,samName}")]
        public IActionResult Delete(string domain, string samName)
        {
            this.groupService.Delete(domain, samName);
            return NoContent();
        }
    }
}

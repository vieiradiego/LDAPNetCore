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
        [HttpGet] //GET api/group
        public IActionResult Get()
        {
            return Ok(this.groupService.FindAll());
        }

        //GET api/group/samName
        [HttpGet("{samName}")]
        public IActionResult Get(string samName)
        {
            var group = this.groupService.FindBySamName(samName);
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

        // DELETE api/values/5
        [HttpDelete("{samName}")]
        public IActionResult Delete(string samName)
        {
            this.groupService.Delete(samName);
            return NoContent();
        }
    }
}

using AgentNetCore.Model;
using AgentNetCore.Service;
using Microsoft.AspNetCore.Mvc;

namespace AgentNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GroupsController : ControllerBase
    {
        private IGroupService _groupService;
        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }
        //GET api/groups
        [HttpGet]
        public IActionResult Get()
        {
            var group = _groupService.FindAll();
            if (group == null) return NotFound();
            return Ok(group);
        }

        //GET api/groups/domain/samName
        [HttpGet("{domain}/{samName}")]
        public IActionResult Get(string domain, string samName)
        {
            var group = this._groupService.FindBySamName(domain, samName);
            if (group == null) return NotFound();
            return Ok(group);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] Group group)
        {
            if (group == null) return BadRequest();
            var newGroup = new ObjectResult(this._groupService.Create(group));
            if (newGroup.Value == null) return Conflict();
            return newGroup;
        }

        // PUT api/values
        [HttpPut]
        public IActionResult Put([FromBody] Group group)
        {
            if (group == null) return BadRequest();
            return new ObjectResult(this._groupService.Update(group));
        }

        // DELETE api/domain/samName/
        [HttpDelete("{domain}/{samName}")]
        public IActionResult Delete(string domain, string samName)
        {
            this._groupService.Delete(domain, samName);
            return NoContent();
        }
    }
}

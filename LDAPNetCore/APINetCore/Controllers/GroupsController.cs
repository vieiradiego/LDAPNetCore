using APINetCore.Model;
using APINetCore.Service;
using Microsoft.AspNetCore.Mvc;

namespace APINetCore.Controllers
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

        [HttpGet("{id}")] //GET api/group/id
        public IActionResult Get(long id)
        {
            var group = this.groupService.FindById(id);
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
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this.groupService.Delete(id);
            return NoContent();
        }
    }
}

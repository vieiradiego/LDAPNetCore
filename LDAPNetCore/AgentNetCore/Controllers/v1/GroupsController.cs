using AgentNetCore.Data.VO;
using AgentNetCore.Model;
using AgentNetCore.Service;
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
        public GroupsController(IGroupService groupService, ILogger<UsersController> logger)
        {
            _groupService = groupService;
            _logger = logger;
        }
        //GET api/groups
        [HttpGet]
        [SwaggerResponse((200), Type = typeof(List<GroupVO>))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get()
        {
            var group = _groupService.FindAll();
            if (group == null) return NotFound();
            return Ok(group);
        }

        //GET api/groups/domain/samName
        [HttpGet("{domain}/{samName}")]
        [SwaggerResponse((200), Type = typeof(GroupVO))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get(string domain, string samName)
        {
            var group = this._groupService.FindBySamName(domain, samName);
            if (group == null) return NotFound();
            return Ok(group);
        }

        // POST api/values
        [HttpPost]
        [SwaggerResponse((201), Type = typeof(GroupVO))]
        [SwaggerResponse(209)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Post([FromBody] GroupVO group)
        {
            if (group == null) return BadRequest();
            var newGroup = new ObjectResult(this._groupService.Create(group));
            if (newGroup.Value == null) return Conflict();
            return newGroup;
        }

        // PUT api/values
        [HttpPut]
        [SwaggerResponse((202), Type = typeof(GroupVO))]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Put([FromBody] GroupVO group)
        {
            if (group == null) return BadRequest();
            return new ObjectResult(this._groupService.Update(group));
        }

        // DELETE api/domain/samName/
        [HttpDelete("{domain}/{samName}")]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Delete(string domain, string samName)
        {
            this._groupService.Delete(domain, samName);
            return NoContent();
        }
    }
}

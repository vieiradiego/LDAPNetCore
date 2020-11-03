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
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private readonly ILogger _logger;
        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;

        }
        // GET api/users
        [HttpGet]
        [SwaggerResponse((200), Type = typeof(List<UserVO>))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get()
        {
            _logger.LogInformation(this.ControllerContext.RouteData.Values["controller"].ToString() + "|" +
                                   this.ControllerContext.RouteData.Values["action"].ToString() + "|" +
                                   this.ControllerContext.RouteData.Values["version"].ToString() + "|" +
                                   Request.Host + "|" +
                                   System.DateTime.Now.ToString("dd-MMM-yyyy-HH:mm:ss")
                                   );
            return Ok(_userService.FindAll());
        }

        // GET api/users/email
        [HttpGet("{email}")]
        [SwaggerResponse((200), Type = typeof(UserVO))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [SwaggerResponse(404)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get(string email)
        {
            var user = _userService.FindByEmail(email);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // POST api/users
        [HttpPost]
        [SwaggerResponse((201), Type = typeof(UserVO))]
        [SwaggerResponse(209)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Post([FromBody] UserVO user)
        {
            if (user == null) return BadRequest();
            var newUser = new ObjectResult(_userService.Create(user));
            if (newUser.Value == null) return Conflict();
            return newUser;
        }

        // PUT api/users
        [HttpPut]
        [SwaggerResponse((202), Type = typeof(UserVO))]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Put([FromBody] UserVO user)
        {
            if (user == null) return BadRequest();
            return new ObjectResult(_userService.Update(user));
        }

        // DELETE api/values/domain/email
        [HttpDelete("{domain}/{samName}")]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Delete(string domain, string email)
        {
            _userService.Delete(domain, email);
            return NoContent();
        }
    }
}

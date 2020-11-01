using AgentNetCore.Model;
using AgentNetCore.Service;
using Microsoft.AspNetCore.Mvc;

namespace AgentNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        // GET api/users
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_userService.FindAll());
        }
        
        // GET api/users/email
        [HttpGet("{email}")]
        public IActionResult Get(string email)
        {
            var user = _userService.FindByEmail(email);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // POST api/users
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            if (user == null) return BadRequest();
            var newUser = new ObjectResult(_userService.Create(user));
            if (newUser.Value == null) return Conflict();
            return newUser;
        }

        // PUT api/users
        [HttpPut]
        public IActionResult Put([FromBody] User user)
        {
            if (user == null) return BadRequest();
            return new ObjectResult(_userService.Update(user));
        }

        // DELETE api/values/domain/email
        [HttpDelete("{domain}/{samName}")]
        public IActionResult Delete(string domain, string email)
        {
            _userService.Delete(domain, email);
            return NoContent();
        }
    }
}

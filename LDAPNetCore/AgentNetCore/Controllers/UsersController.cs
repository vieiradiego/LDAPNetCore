using AgentNetCore.Model;
using AgentNetCore.Service;
using Microsoft.AspNetCore.Mvc;

namespace AgentNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService userService;
        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }
        // GET api/users
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.userService.FindAll());
        }
        
        // GET api/users/email
        [HttpGet("{email}")]
        public IActionResult Get(string email)
        {
            var user = this.userService.FindByEmail(email);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // POST api/users
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            if (user == null) return BadRequest();
            var newUser = new ObjectResult(this.userService.Create(user));
            if (newUser.Value == null) return Conflict();
            return newUser;
        }

        // PUT api/users
        [HttpPut]
        public IActionResult Put([FromBody] User user)
        {
            if (user == null) return BadRequest();
            return new ObjectResult(this.userService.Update(user));
        }

        // DELETE api/values/domain/email
        [HttpDelete("{email}")]
        public IActionResult Delete(string email)
        {
            this.userService.Delete(email);
            return NoContent();
        }
    }
}

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

        // GET api/users/domain
        [HttpGet("{domain}")]
        public IActionResult Get(string domain)
        {
            return Ok(this.userService.FindAll(domain));
        }

        // GET api/users/domain,email
        [HttpGet("{domain, email}")]
        public IActionResult Get(string domain, string email)
        {
            var person = this.userService.FindByEmail(domain, email);
            if (person == null) return NotFound();
            return Ok(person);
        }

        // POST api/users
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            if (user == null) return BadRequest();
            return new ObjectResult(this.userService.Create(user));
        }

        // PUT api/users
        [HttpPut]
        public IActionResult Put([FromBody] User user)
        {
            if (user == null) return BadRequest();
            return new ObjectResult(this.userService.Update(user));
        }

        // DELETE api/values/domain&mail
        [HttpDelete("{domain, email}")]
        public IActionResult Delete(string domain, string email)
        {
            this.userService.Delete(domain, email);
            return NoContent();
        }
    }
}

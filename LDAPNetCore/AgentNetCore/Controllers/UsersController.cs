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

        // GET api/users/1
        [HttpGet("{email}")]
        public IActionResult Get(string email)
        {
            var person = this.userService.FindByEmail(email);
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

        // DELETE api/values/5
        [HttpDelete("{email}")]
        public IActionResult Delete(string email)
        {
            this.userService.Delete(email);
            return NoContent();
        }
    }
}

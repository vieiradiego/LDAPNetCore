using AgentNetCore.Model;
using Microsoft.AspNetCore.Mvc;
using Persistence.Interface;

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

        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.userService.FindAll());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var person = this.userService.FindById(id);
            if (person == null) return NotFound();
            return Ok(person);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            if (user == null) return BadRequest();
            return new ObjectResult(this.userService.Create(user));
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put([FromBody] User user)
        {
            if (user == null) return BadRequest();
            return new ObjectResult(this.userService.Create(user));
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this.userService.Delete(id);
            return NoContent();
        }
    }
}

using APINetCore.Model;
using APINetCore.Service;
using Microsoft.AspNetCore.Mvc;

namespace APINetCore.Controllers
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
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var person = this.userService.FindById(id);
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
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this.userService.Delete(id);
            return NoContent();
        }
    }
}

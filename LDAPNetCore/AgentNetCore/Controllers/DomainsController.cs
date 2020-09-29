using AgentNetCore.Model;
using AgentNetCore.Service;
using Microsoft.AspNetCore.Mvc;


namespace AgentNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DomainsController : ControllerBase
    {
        private IDomainService domainService;
        public DomainsController(IDomainService domainService)
        {
            this.domainService = domainService;
        }
        [HttpGet] //GET api/group
        public IActionResult Get()
        {
            return Ok(this.domainService.FindAll());
        }

        [HttpGet("{id}")] //GET api/group/id
        public IActionResult Get(long id)
        {
            var domain = this.domainService.FindById(id);
            if (domain == null) return NotFound();
            return Ok(domain);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] Domain domain)
        {
            if (domain == null) return BadRequest();
            return new ObjectResult(this.domainService.Create(domain));
        }

        // PUT api/values
        [HttpPut]
        public IActionResult Put([FromBody] Domain domain)
        {
            if (domain == null) return BadRequest();
            return new ObjectResult(this.domainService.Update(domain));
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this.domainService.Delete(id);
            return NoContent();
        }

    }
}

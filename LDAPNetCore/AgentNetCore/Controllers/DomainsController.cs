using Microsoft.AspNetCore.Mvc;
using Persistence.Interface;

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

    }
}

using AgentNetCore.Model;
using AgentNetCore.Service;
using Microsoft.AspNetCore.Mvc;

namespace AgentNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ForestsController : ControllerBase
    {
        private IForestService _forestService;
        public ForestsController(IForestService forestService)
        {
            _forestService = forestService;
        }
        //GET api/forests
        [HttpGet]
        public IActionResult Get()
        {
            var forest = _forestService.FindAll();
            if (forest == null) return NotFound();
            return Ok(forest);
        }

        //GET api/forests/domain
        [HttpGet("{domain}")]
        public IActionResult Get(string domain)
        {
            var forest = _forestService.FindAll(domain);
            if (forest == null) return NotFound();
            return Ok(forest);
        }
    }
}

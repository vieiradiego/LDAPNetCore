using AgentNetCore.Data.VO;
using AgentNetCore.Model;
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
    public class ForestsController : ControllerBase
    {
        private IForestService _forestService;
        private readonly ILogger _logger;
        public ForestsController(IForestService forestService, ILogger<UsersController> logger)
        {
            _forestService = forestService;
            _logger = logger;
        }
        //GET api/forests
        [HttpGet]
        [SwaggerResponse((200), Type = typeof(List<ForestVO>))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get()
        {
            var forest = _forestService.FindAll();
            if (forest == null) return NotFound();
            return Ok(forest);
        }

        //GET api/forests/domain
        [HttpGet("{domain}")]
        [SwaggerResponse((200), Type = typeof(ForestVO))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get(string domain)
        {
            var forest = _forestService.FindAll(domain);
            if (forest == null) return NotFound();
            return Ok(forest);
        }
    }
}

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
    public class ForestsController : ControllerBase, IController
    {
        private IForestService _forestService;
        private readonly ILogger _logger;
        public ForestsController(IForestService forestService, ILogger<UsersController> logger)
        {
            _forestService = forestService;
            _logger = logger;
        }
        /// <summary>
        /// RECUPERAR todos os objetos do Domínios configurados
        /// </summary>
        /// <remarks>
        /// Retorna uma lista de objetos no formato ForestVO
        /// </remarks>
        /// <returns>O retorno desse serviço é uma lista de ForestVO </returns>
        /// <param name="dn"></param>
        [HttpGet]
        [SwaggerResponse((200), Type = typeof(List<ForestVO>))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get([FromQuery] string dn)
        {
            if (!string.IsNullOrWhiteSpace(dn))
            {
                var forest = _forestService.FindAll(dn);
                if (forest == null) return NotFound();
                return Ok(forest);
            }
            else
            {
                return Ok(_forestService.FindAll());
            }
        }
    }
}

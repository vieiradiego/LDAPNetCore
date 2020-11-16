﻿using AgentNetCore.Data.VO;
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

        /// <summary>
        /// RECUPERAR os dados para um determinado Domínio
        /// </summary>
        /// <remarks>
        /// Retorna um objeto no formato ForestVO
        /// </remarks>
        /// <returns>O retorno desse serviço é um determinado ForestVO encontrado</returns>
        /// <param name="domain"></param>
        [HttpGet("domain")]
        [SwaggerResponse((200), Type = typeof(ForestVO))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get([FromQuery] string domain)
        {
            var forest = _forestService.FindAll(domain);
            if (forest == null) return NotFound();
            return Ok(forest);
        }
    }
}

using AgentNetCore.Data.VO;
using AgentNetCore.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using Tapioca.HATEOAS;

namespace AgentNetCore.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class ComputersController : ControllerBase, IController
    {
        private IComputerService _computerService;
        private readonly ILogger _logger;
        public ComputersController(IComputerService computerService)
        {
            _computerService = computerService;
        }

        /// <summary>
        /// RECUPERAR todos os Computadores
        /// </summary>
        /// <remarks>
        /// Retorna uma lista de objetos no formato ComputerVO
        /// </remarks>
        /// <returns>O retorno desse serviço é uma lista de ComputerVO </returns>
        [HttpGet]
        [SwaggerResponse((200), Type = typeof(List<ComputerVO>))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get([FromQuery] string dn)
        {
            if (!string.IsNullOrWhiteSpace(dn))
            {
                var user = _computerService.FindByDn(dn);
                if (user == null) return NotFound();
                return Ok(user);
            }
            else
            {
                return Ok(_computerService.FindAll());
            }
        }

        /// <summary>
        /// RECUPERAR os dados para um determinado Computador
        /// </summary>
        /// <remarks>
        /// Retorna um objeto no formato ComputerVO
        /// </remarks>
        /// <returns>O retorno desse serviço é um determinado ComputerVO encontrado</returns>
        /// <param name="dn"></param>
        /// <param name="samName"></param>
        [HttpGet("find")]
        [SwaggerResponse((200), Type = typeof(GroupVO))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [SwaggerResponse(404)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetBySamName([FromQuery] string dn, [FromQuery] string samName)
        {
            if (!string.IsNullOrWhiteSpace(dn) && !string.IsNullOrWhiteSpace(samName))
            {
                var computer = _computerService.FindBySamName(dn, samName);
                if (computer == null) return NotFound();
                return Ok(computer);
            }
            else
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// CRIAR um Computador
        /// </summary>
        /// <remarks>
        /// Retorna um objeto no formato ComputerVO
        /// </remarks>
        /// <returns>O retorno desse serviço é um ComputerVO criado</returns>
        /// <param name="computer"></param>
        [HttpPost]
        [SwaggerResponse((201), Type = typeof(ComputerVO))]
        [SwaggerResponse(209)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Post([FromBody] ComputerVO computer)
        {
            if (computer == null) return BadRequest();
            var newGroup = new ObjectResult(this._computerService.Create(computer));
            if (newGroup.Value == null) return Conflict();
            return Ok(newGroup);
        }   
        /// <summary>
        /// ATUALIZAR um Computador
        /// </summary>
        /// <remarks>
        /// Retorna um objeto no formato ComputerVO
        /// </remarks>
        /// <returns>O retorno desse serviço é um GroupVO atualizado</returns>
        /// <param name="computer"></param>
        [HttpPut]
        [SwaggerResponse((202), Type = typeof(ComputerVO))]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Put([FromBody] ComputerVO computer)
        {
            if (computer == null) return BadRequest();
            return new ObjectResult(this._computerService.Update(computer));
        }

        /// <summary>
        /// DELETAR um Computador
        /// </summary>
        /// <remarks>
        /// Retorno de HTTP code
        /// </remarks>
        /// <returns>O retorno desse serviço é código HTTP</returns>
        /// <param name="dn"></param>
        /// <param name="samName"></param>
        [HttpDelete]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Delete([FromQuery] string dn, [FromQuery] string samName)
        {
            if (!string.IsNullOrWhiteSpace(dn) && !string.IsNullOrWhiteSpace(samName))
            {
                if (this._computerService.Delete(dn, samName)) return NoContent();
                return Conflict();
            }
            return BadRequest();
        }
    }
}

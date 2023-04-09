using Microsoft.AspNetCore.Mvc;
using N5PermisosAPI.Models;
using N5PermisosAPI.DataAccess.Interfaces;
using MediatR;
using N5PermisosAPI.CQRS.Queries;
using N5PermisosAPI.CQRS.Commands;

namespace N5PermisosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogEvent _logEvent;
        public PermisosController(IMediator mediator, ILogEvent logEvent)
        {
            _mediator = mediator;
            _logEvent = logEvent;      
        }
           

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Permiso>>> GetPermisos()
        {
            try
            {
                var permisos = await _mediator.Send(new GetPermisosQuery());
                //await _logEvent.LogEventToElasticsearchAsync("GetPermisos", permisos);
                //await _logEvent.LogEventToKafkaAsync("get", permisos);
                return Ok(permisos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost]
        public async Task<ActionResult<Permiso>> SolicitarPermiso(Permiso permiso)
        {
            try
            {
                int id = await _mediator.Send(new SolicitarPermisoCommand(permiso));
                //await _logEvent.LogEventToElasticsearchAsync("SolicitarPermiso", permiso);
                //await _logEvent.LogEventToKafkaAsync("request", permiso);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
          
        }

        [HttpPut]
        public async Task<IActionResult> ModificarPermiso(int id, Permiso permiso)
        {           
            try
            {
                bool result = await _mediator.Send(new ModificarPermisoCommand(id, permiso));
                if (result)
                {
                    //await _logEvent.LogEventToElasticsearchAsync("ModificarPermiso", permiso);
                    //await _logEvent.LogEventToKafkaAsync("modify", permiso);
                    return Ok(true);
                }
                else
                {
                    return NotFound();
                }
            }            
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }       
    }
}

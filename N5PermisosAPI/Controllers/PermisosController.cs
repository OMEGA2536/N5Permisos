using Microsoft.AspNetCore.Mvc;
using N5PermisosAPI.Models;
using MediatR;
using N5PermisosAPI.CQRS.Queries;
using N5PermisosAPI.CQRS.Commands;
using CQRS.Commands;

namespace N5PermisosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisosController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PermisosController(IMediator mediator)
        {
            _mediator = mediator;
        }
           

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Permiso>>> GetPermisos()
        {
            try
            {               
                return Ok(await _mediator.Send(new GetPermisosQuery()));
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
                return Ok(await _mediator.Send(new SolicitarPermisoCommand(permiso)));
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
                return Ok(await _mediator.Send(new ModificarPermisoCommand(id, permiso)));
            }            
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("tipos-permiso")]
        public async Task<ActionResult<int>> CrearTipoPermiso(TipoPermiso tipoPermiso)
        {
            try
            {
                return Ok(await _mediator.Send(new CrearTipoPermisoCommand(tipoPermiso)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

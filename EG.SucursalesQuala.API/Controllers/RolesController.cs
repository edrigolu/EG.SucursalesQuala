using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EG.SucursalesQuala.Application.DTOs;
using EG.SucursalesQuala.Application.Interfaces;

namespace EG.SucursalesQuala.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRolService _rolService;

        public RolesController(IRolService rolService)
        {
            _rolService = rolService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolDto>>> Get()
        {
            var roles = await _rolService.ObtenerTodosAsync();
            return Ok(roles);
        }

        [HttpGet("activos")]
        public async Task<ActionResult<IEnumerable<RolDto>>> GetActivos()
        {
            var roles = await _rolService.ObtenerActivosAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RolDto>> Get(int id)
        {
            var rol = await _rolService.ObtenerPorIdAsync(id);
            if (rol == null)
                return NotFound();

            return Ok(rol);
        }

        [HttpPost]
        public async Task<ActionResult<RolDto>> Post([FromBody] CrearRolDto crearRolDto)
        {
            try
            {
                var rolCreado = await _rolService.CrearAsync(crearRolDto);
                return CreatedAtAction(nameof(Get), new { id = rolCreado.Id }, rolCreado);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al crear el rol" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ActualizarRolDto actualizarRolDto)
        {
            if (id != actualizarRolDto.Id)
                return BadRequest(new { message = "ID no coincide" });

            try
            {
                await _rolService.ActualizarAsync(actualizarRolDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al actualizar el rol" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _rolService.EliminarAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al eliminar el rol" });
            }
        }

        [HttpGet("existe-codigo/{codigo}")]
        public async Task<ActionResult<bool>> ExisteCodigo(string codigo, [FromQuery] int excluirId = 0)
        {
            var existe = await _rolService.ExisteCodigoAsync(codigo, excluirId);
            return Ok(existe);
        }
    }
}
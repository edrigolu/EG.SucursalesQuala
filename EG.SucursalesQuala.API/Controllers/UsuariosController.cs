using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EG.SucursalesQuala.Application.DTOs;
using EG.SucursalesQuala.Application.Interfaces;

namespace EG.SucursalesQuala.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> Get()
        {
            var usuarios = await _usuarioService.ObtenerTodosAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> Get(int id)
        {
            var usuario = await _usuarioService.ObtenerPorIdAsync(id);
            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioDto>> Post([FromBody] CrearUsuarioDto crearUsuarioDto)
        {
            try
            {
                var usuarioCreado = await _usuarioService.CrearAsync(crearUsuarioDto);
                return CreatedAtAction(nameof(Get), new { id = usuarioCreado.Id }, usuarioCreado);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Error al crear el usuario" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ActualizarUsuarioDto actualizarUsuarioDto)
        {
            if (id != actualizarUsuarioDto.Id)
                return BadRequest(new { message = "ID no coincide" });

            try
            {
                await _usuarioService.ActualizarAsync(actualizarUsuarioDto);
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
            catch (Exception)
            {
                return BadRequest(new { message = "Error al actualizar el usuario" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _usuarioService.EliminarAsync(id);
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Error al eliminar el usuario" });
            }
        }

        [HttpGet("existe-correo/{correo}")]
        public async Task<ActionResult<bool>> ExisteCorreo(string correo, [FromQuery] int excluirId = 0)
        {
            var existe = await _usuarioService.ExisteCorreoAsync(correo, excluirId);
            return Ok(existe);
        }

        [HttpGet("existe-codigo/{codigo}")]
        public async Task<ActionResult<bool>> ExisteCodigo(string codigo, [FromQuery] int excluirId = 0)
        {
            var existe = await _usuarioService.ExisteCodigoAsync(codigo, excluirId);
            return Ok(existe);
        }
    }
}
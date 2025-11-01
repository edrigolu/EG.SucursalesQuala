using EG.SucursalesQuala.Application.DTOs;
using EG.SucursalesQuala.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace EG.SucursalesQuala.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SucursalesController : ControllerBase
    {
        private readonly ISucursalService _sucursalService;

        public SucursalesController(ISucursalService sucursalService)
        {
            _sucursalService = sucursalService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SucursalDto>>> Get()
        {
            var sucursales = await _sucursalService.ObtenerTodosAsync();
            return Ok(sucursales);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SucursalDto>> Get(int id)
        {
            var sucursal = await _sucursalService.ObtenerPorIdAsync(id);
            if (sucursal == null)
            {
                return NotFound();
            }

            return Ok(sucursal);
        }

        [HttpPost]
        public async Task<ActionResult<SucursalDto>> Post([FromBody] CrearSucursalDto crearSucursalDto)
        {
            try
            {
                // Validar que la fecha no sea anterior a la actual
                if (crearSucursalDto.FechaCreacion < DateTime.Today)
                {
                    return BadRequest(new { message = "La fecha de creación no puede ser anterior a la fecha actual" });
                }

                var sucursalCreada = await _sucursalService.CrearAsync(crearSucursalDto);
                return CreatedAtAction(nameof(Get), new { id = sucursalCreada.Id }, sucursalCreada);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Error al crear la sucursal" });
            }
        }

      

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ActualizarSucursalDto actualizarSucursalDto)
        {
            Console.WriteLine($"ID recibido: {id}");
            Console.WriteLine($"ID en DTO: {actualizarSucursalDto.Id}");
            Console.WriteLine($"Datos recibidos: {JsonSerializer.Serialize(actualizarSucursalDto)}");

            if (id != actualizarSucursalDto.Id)
            {
                return BadRequest(new { message = "ID no coincide" });
            }

            try
            {
                await _sucursalService.ActualizarAsync(actualizarSucursalDto);
                return NoContent();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error SQL: {ex.Message}");
                return BadRequest(new { message = $"Error de base de datos: {ex.Message}" });
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
                Console.WriteLine($"Error general: {ex.Message}");
                return BadRequest(new { message = "Error al actualizar la sucursal" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _sucursalService.EliminarAsync(id);
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Error al eliminar la sucursal" });
            }
        }

        [HttpGet("existe-codigo/{codigo}")]
        public async Task<ActionResult<bool>> ExisteCodigo(string codigo, [FromQuery] int excluirId = 0)
        {
            var existe = await _sucursalService.ExisteCodigoAsync(codigo, excluirId);
            return Ok(existe);
        }

        [HttpGet("existe-identificacion/{identificacion}")]
        public async Task<ActionResult<bool>> ExisteIdentificacion(string identificacion, [FromQuery] int excluirId = 0)
        {
            var existe = await _sucursalService.ExisteIdentificacionAsync(identificacion, excluirId);
            return Ok(existe);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EG.SucursalesQuala.Application.DTOs;
using EG.SucursalesQuala.Application.Interfaces;

namespace EG.SucursalesQuala.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MonedasController : ControllerBase
    {
        private readonly IMonedaService _monedaService;

        public MonedasController(IMonedaService monedaService)
        {
            _monedaService = monedaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MonedaDto>>> Get()
        {
            var monedas = await _monedaService.ObtenerTodosAsync();
            return Ok(monedas);
        }

        [HttpGet("activas")]
        public async Task<ActionResult<IEnumerable<MonedaDto>>> GetActivas()
        {
            var monedas = await _monedaService.ObtenerActivasAsync();
            return Ok(monedas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MonedaDto>> Get(int id)
        {
            var moneda = await _monedaService.ObtenerPorIdAsync(id);
            if (moneda == null)
                return NotFound();

            return Ok(moneda);
        }

        [HttpPost]
        public async Task<ActionResult<MonedaDto>> Post([FromBody] CrearMonedaDto crearMonedaDto)
        {
            try
            {
                var monedaCreada = await _monedaService.CrearAsync(crearMonedaDto);
                return CreatedAtAction(nameof(Get), new { id = monedaCreada.Id }, monedaCreada);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Error al crear la moneda" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ActualizarMonedaDto actualizarMonedaDto)
        {
            if (id != actualizarMonedaDto.Id)
                return BadRequest(new { message = "ID no coincide" });

            try
            {
                await _monedaService.ActualizarAsync(actualizarMonedaDto);
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
                return BadRequest(new { message = "Error al actualizar la moneda" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _monedaService.EliminarAsync(id);
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Error al eliminar la moneda" });
            }
        }

        [HttpGet("existe-codigo/{codigo}")]
        public async Task<ActionResult<bool>> ExisteCodigo(string codigo, [FromQuery] int excluirId = 0)
        {
            var existe = await _monedaService.ExisteCodigoAsync(codigo, excluirId);
            return Ok(existe);
        }
    }
}
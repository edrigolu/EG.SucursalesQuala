using EG.SucursalesQuala.Application.DTOs;
using EG.SucursalesQuala.Application.Interfaces;
using EG.SucursalesQuala.Domain.Entities;
using EG.SucursalesQuala.Domain.Interfaces;

namespace EG.SucursalesQuala.Application.Services
{
    public class MonedaService : IMonedaService
    {
        private readonly IRepositorioMoneda _repositorioMoneda;

        public MonedaService(IRepositorioMoneda repositorioMoneda)
        {
            _repositorioMoneda = repositorioMoneda;
        }

        public async Task<IEnumerable<MonedaDto>> ObtenerTodosAsync()
        {
            IEnumerable<Moneda> monedas = await _repositorioMoneda.ObtenerTodosAsync();
            return monedas.Select(m => new MonedaDto
            {
                Id = m.Id,
                Codigo = m.Codigo,
                Nombre = m.Nombre,
                Simbolo = m.Simbolo,
                Estado = m.Estado
            });
        }

        public async Task<IEnumerable<MonedaDto>> ObtenerActivasAsync()
        {
            IEnumerable<Moneda> monedas = await _repositorioMoneda.ObtenerActivasAsync();
            return monedas.Select(m => new MonedaDto
            {
                Id = m.Id,
                Codigo = m.Codigo,
                Nombre = m.Nombre,
                Simbolo = m.Simbolo,
                Estado = m.Estado
            });
        }

        public async Task<MonedaDto?> ObtenerPorIdAsync(int id)
        {
            Moneda? moneda = await _repositorioMoneda.ObtenerPorIdAsync(id);
            if (moneda == null)
            {
                return null;
            }

            return new MonedaDto
            {
                Id = moneda.Id,
                Codigo = moneda.Codigo,
                Nombre = moneda.Nombre,
                Simbolo = moneda.Simbolo,
                Estado = moneda.Estado
            };
        }

        public async Task<MonedaDto> CrearAsync(CrearMonedaDto crearMonedaDto)
        {
            if (await _repositorioMoneda.ExisteCodigoAsync(crearMonedaDto.Codigo))
            {
                throw new InvalidOperationException("El código de moneda ya existe");
            }

            Moneda moneda = new Moneda
            {
                Codigo = crearMonedaDto.Codigo,
                Nombre = crearMonedaDto.Nombre,
                Simbolo = crearMonedaDto.Simbolo,
                Estado = true
            };

            Moneda monedaCreada = await _repositorioMoneda.CrearAsync(moneda);

            return new MonedaDto
            {
                Id = monedaCreada.Id,
                Codigo = monedaCreada.Codigo,
                Nombre = monedaCreada.Nombre,
                Simbolo = monedaCreada.Simbolo,
                Estado = monedaCreada.Estado
            };
        }

        public async Task ActualizarAsync(ActualizarMonedaDto actualizarMonedaDto)
        {
            if (await _repositorioMoneda.ExisteCodigoAsync(actualizarMonedaDto.Codigo, actualizarMonedaDto.Id))
            {
                throw new InvalidOperationException("El código de moneda ya existe");
            }

            Moneda? monedaExistente = await _repositorioMoneda.ObtenerPorIdAsync(actualizarMonedaDto.Id);
            if (monedaExistente == null)
            {
                throw new KeyNotFoundException("Moneda no encontrada");
            }

            monedaExistente.Codigo = actualizarMonedaDto.Codigo;
            monedaExistente.Nombre = actualizarMonedaDto.Nombre;
            monedaExistente.Simbolo = actualizarMonedaDto.Simbolo;
            monedaExistente.Estado = actualizarMonedaDto.Estado;

            await _repositorioMoneda.ActualizarAsync(monedaExistente);
        }

        public async Task EliminarAsync(int id)
        {
            await _repositorioMoneda.EliminarAsync(id);
        }

        public async Task<bool> ExisteCodigoAsync(string codigo, int excluirId = 0)
        {
            return await _repositorioMoneda.ExisteCodigoAsync(codigo, excluirId);
        }
    }
}
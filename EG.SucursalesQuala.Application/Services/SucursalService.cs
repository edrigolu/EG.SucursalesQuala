using EG.SucursalesQuala.Application.DTOs;
using EG.SucursalesQuala.Application.Interfaces;
using EG.SucursalesQuala.Domain.Entities;
using EG.SucursalesQuala.Domain.Interfaces;

namespace EG.SucursalesQuala.Application.Services
{
    public class SucursalService : ISucursalService
    {
        private readonly IRepositorioSucursal _repositorioSucursal;

        public SucursalService(IRepositorioSucursal repositorioSucursal)
        {
            _repositorioSucursal = repositorioSucursal;
        }

        public async Task<IEnumerable<SucursalDto>> ObtenerTodosAsync()
        {
            IEnumerable<Sucursal> sucursales = await _repositorioSucursal.ObtenerConMonedaAsync();
            return sucursales.Select(s => new SucursalDto
            {
                Id = s.Id,
                Codigo = s.Codigo,
                Nombre = s.Nombre,
                Descripcion = s.Descripcion,
                Direccion = s.Direccion,
                Identificacion = s.Identificacion,
                FechaCreacion = s.FechaCreacion,
                IdMoneda = s.IdMoneda,
                NombreMoneda = s.Moneda?.Nombre,
                SimboloMoneda = s.Moneda?.Simbolo,
                Estado = s.Estado
            });
        }

        public async Task<SucursalDto?> ObtenerPorIdAsync(int id)
        {
            Sucursal? sucursal = await _repositorioSucursal.ObtenerPorIdConMonedaAsync(id);
            if (sucursal == null) return null;

            return new SucursalDto
            {
                Id = sucursal.Id,
                Codigo = sucursal.Codigo,
                Nombre = sucursal.Nombre,
                Descripcion = sucursal.Descripcion,
                Direccion = sucursal.Direccion,
                Identificacion = sucursal.Identificacion,
                FechaCreacion = sucursal.FechaCreacion,
                IdMoneda = sucursal.IdMoneda,
                NombreMoneda = sucursal.Moneda?.Nombre,
                SimboloMoneda = sucursal.Moneda?.Simbolo,
                Estado = sucursal.Estado
            };
        }

        public async Task<SucursalDto> CrearAsync(CrearSucursalDto crearSucursalDto)
        {
            if (await _repositorioSucursal.ExisteCodigoAsync(crearSucursalDto.Codigo))
                throw new InvalidOperationException("El código de sucursal ya existe");

            if (await _repositorioSucursal.ExisteIdentificacionAsync(crearSucursalDto.Identificacion))
                throw new InvalidOperationException("La identificación ya existe");

            Sucursal sucursal = new Sucursal
            {
                Codigo = crearSucursalDto.Codigo,
                Nombre = crearSucursalDto.Nombre,
                Descripcion = crearSucursalDto.Descripcion,
                Direccion = crearSucursalDto.Direccion,
                Identificacion = crearSucursalDto.Identificacion,
                FechaCreacion = crearSucursalDto.FechaCreacion,
                IdMoneda = crearSucursalDto.IdMoneda,
                Estado = true
            };

            Sucursal sucursalCreada = await _repositorioSucursal.CrearAsync(sucursal);

            return new SucursalDto
            {
                Id = sucursalCreada.Id,
                Codigo = sucursalCreada.Codigo,
                Nombre = sucursalCreada.Nombre,
                Descripcion = sucursalCreada.Descripcion,
                Direccion = sucursalCreada.Direccion,
                Identificacion = sucursalCreada.Identificacion,
                FechaCreacion = sucursalCreada.FechaCreacion,
                IdMoneda = sucursalCreada.IdMoneda,
                Estado = sucursalCreada.Estado
            };
        }

        public async Task ActualizarAsync(ActualizarSucursalDto actualizarSucursalDto)
        {
            if (await _repositorioSucursal.ExisteCodigoAsync(actualizarSucursalDto.Codigo, actualizarSucursalDto.Id))
                throw new InvalidOperationException("El código de sucursal ya existe");

            if (await _repositorioSucursal.ExisteIdentificacionAsync(actualizarSucursalDto.Identificacion, actualizarSucursalDto.Id))
                throw new InvalidOperationException("La identificación ya existe");

            Sucursal? sucursalExistente = await _repositorioSucursal.ObtenerPorIdAsync(actualizarSucursalDto.Id);
            if (sucursalExistente == null)
                throw new KeyNotFoundException("Sucursal no encontrada");

            sucursalExistente.Codigo = actualizarSucursalDto.Codigo;
            sucursalExistente.Nombre = actualizarSucursalDto.Nombre;
            sucursalExistente.Descripcion = actualizarSucursalDto.Descripcion;
            sucursalExistente.Direccion = actualizarSucursalDto.Direccion;
            sucursalExistente.Identificacion = actualizarSucursalDto.Identificacion;
            sucursalExistente.FechaCreacion = actualizarSucursalDto.FechaCreacion;
            sucursalExistente.IdMoneda = actualizarSucursalDto.IdMoneda;
            sucursalExistente.Estado = actualizarSucursalDto.Estado;

            await _repositorioSucursal.ActualizarAsync(sucursalExistente);
        }

        public async Task EliminarAsync(int id)
        {
            await _repositorioSucursal.EliminarAsync(id);
        }

        public async Task<bool> ExisteCodigoAsync(string codigo, int excluirId = 0)
        {
            return await _repositorioSucursal.ExisteCodigoAsync(codigo, excluirId);
        }

        public async Task<bool> ExisteIdentificacionAsync(string identificacion, int excluirId = 0)
        {
            return await _repositorioSucursal.ExisteIdentificacionAsync(identificacion, excluirId);
        }
    }
}
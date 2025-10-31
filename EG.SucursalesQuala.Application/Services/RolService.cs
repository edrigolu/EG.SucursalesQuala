using EG.SucursalesQuala.Application.DTOs;
using EG.SucursalesQuala.Application.Interfaces;
using EG.SucursalesQuala.Domain.Entities;
using EG.SucursalesQuala.Domain.Interfaces;

namespace EG.SucursalesQuala.Application.Services
{
    public class RolService : IRolService
    {
        private readonly IRepositorioRol _repositorioRol;

        public RolService(IRepositorioRol repositorioRol)
        {
            _repositorioRol = repositorioRol;
        }

        public async Task<IEnumerable<RolDto>> ObtenerTodosAsync()
        {
            IEnumerable<Rol> roles = await _repositorioRol.ObtenerTodosAsync();
            return roles.Select(r => new RolDto
            {
                Id = r.Id,
                Codigo = r.Codigo,
                Nombre = r.Nombre,
                Estado = r.Estado
            });
        }

        public async Task<IEnumerable<RolDto>> ObtenerActivosAsync()
        {
            IEnumerable<Rol> roles = await _repositorioRol.ObtenerActivosAsync();
            return roles.Select(r => new RolDto
            {
                Id = r.Id,
                Codigo = r.Codigo,
                Nombre = r.Nombre,
                Estado = r.Estado
            });
        }

        public async Task<RolDto?> ObtenerPorIdAsync(int id)
        {
            Rol? rol = await _repositorioRol.ObtenerPorIdAsync(id);
            if (rol == null)
            {
                return null;
            }

            return new RolDto
            {
                Id = rol.Id,
                Codigo = rol.Codigo,
                Nombre = rol.Nombre,
                Estado = rol.Estado
            };
        }

        public async Task<RolDto> CrearAsync(CrearRolDto crearRolDto)
        {
            if (await _repositorioRol.ExisteCodigoAsync(crearRolDto.Codigo))
            {
                throw new InvalidOperationException("El código de rol ya existe");
            }

            Rol rol = new Rol
            {
                Codigo = crearRolDto.Codigo,
                Nombre = crearRolDto.Nombre,
                Estado = true
            };

            Rol rolCreado = await _repositorioRol.CrearAsync(rol);

            return new RolDto
            {
                Id = rolCreado.Id,
                Codigo = rolCreado.Codigo,
                Nombre = rolCreado.Nombre,
                Estado = rolCreado.Estado
            };
        }

        public async Task ActualizarAsync(ActualizarRolDto actualizarRolDto)
        {
            if (await _repositorioRol.ExisteCodigoAsync(actualizarRolDto.Codigo, actualizarRolDto.Id))
            {
                throw new InvalidOperationException("El código de rol ya existe");
            }

            var rolExistente = await _repositorioRol.ObtenerPorIdAsync(actualizarRolDto.Id);
            if (rolExistente == null)
            {
                throw new KeyNotFoundException("Rol no encontrado");
            }

            rolExistente.Codigo = actualizarRolDto.Codigo;
            rolExistente.Nombre = actualizarRolDto.Nombre;
            rolExistente.Estado = actualizarRolDto.Estado;

            await _repositorioRol.ActualizarAsync(rolExistente);
        }

        public async Task EliminarAsync(int id)
        {
            await _repositorioRol.EliminarAsync(id);
        }

        public async Task<bool> ExisteCodigoAsync(string codigo, int excluirId = 0)
        {
            return await _repositorioRol.ExisteCodigoAsync(codigo, excluirId);
        }
    }
}
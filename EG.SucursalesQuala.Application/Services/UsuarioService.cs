using EG.SucursalesQuala.Application.DTOs;
using EG.SucursalesQuala.Application.Interfaces;
using EG.SucursalesQuala.Domain.Entities;
using EG.SucursalesQuala.Domain.Interfaces;

namespace EG.SucursalesQuala.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IRepositorioUsuario _repositorioUsuario;
        private readonly IEncriptacionService _encriptacionService;

        public UsuarioService(
            IRepositorioUsuario repositorioUsuario,
            IEncriptacionService encriptacionService)
        {
            _repositorioUsuario = repositorioUsuario;
            _encriptacionService = encriptacionService;
        }

        public async Task<IEnumerable<UsuarioDto>> ObtenerTodosAsync()
        {
            IEnumerable<Usuario> usuarios = await _repositorioUsuario.ObtenerTodosAsync();
            return usuarios.Select(u => new UsuarioDto
            {
                Id = u.Id,
                Codigo = u.Codigo,
                Nombre = u.Nombre,
                Correo = u.Correo,
                IdRol = u.IdRol,
                NombreRol = u.Rol?.Nombre,
                Estado = u.Estado
            });
        }

        public async Task<UsuarioDto?> ObtenerPorIdAsync(int id)
        {
            var usuario = await _repositorioUsuario.ObtenerPorIdAsync(id);
            if (usuario == null)
            {
                return null;
            }

            return new UsuarioDto
            {
                Id = usuario.Id,
                Codigo = usuario.Codigo,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                IdRol = usuario.IdRol,
                NombreRol = usuario.Rol?.Nombre,
                Estado = usuario.Estado
            };
        }

       
        public async Task<UsuarioDto> CrearAsync(CrearUsuarioDto crearUsuarioDto)
        {
            try
            {
                if (await _repositorioUsuario.ExisteEmailAsync(crearUsuarioDto.Correo))
                {
                    throw new InvalidOperationException("El correo ya está registrado");
                }

                if (await _repositorioUsuario.ExisteCodigoAsync(crearUsuarioDto.Codigo))
                {
                    throw new InvalidOperationException("El código ya existe");
                }

                var claveEncriptada = _encriptacionService.Encriptar(crearUsuarioDto.Clave);

                var usuario = new Usuario
                {
                    Codigo = crearUsuarioDto.Codigo,
                    Nombre = crearUsuarioDto.Nombre,
                    Correo = crearUsuarioDto.Correo,
                    Clave = claveEncriptada,
                    IdRol = crearUsuarioDto.IdRol,
                    Estado = true
                };

                Console.WriteLine($"Intentando crear usuario: {usuario.Codigo}, {usuario.Correo}");

                var usuarioCreado = await _repositorioUsuario.CrearAsync(usuario);
                Console.WriteLine($"Usuario creado con ID: {usuarioCreado.Id}");

                return new UsuarioDto
                {
                    Id = usuarioCreado.Id,
                    Codigo = usuarioCreado.Codigo,
                    Nombre = usuarioCreado.Nombre,
                    Correo = usuarioCreado.Correo,
                    IdRol = usuarioCreado.IdRol,
                    Estado = usuarioCreado.Estado
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR en CrearAsync: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task ActualizarAsync(ActualizarUsuarioDto actualizarUsuarioDto)
        {
            if (await _repositorioUsuario.ExisteEmailAsync(actualizarUsuarioDto.Correo, actualizarUsuarioDto.Id))
            {
                throw new InvalidOperationException("El correo ya está registrado");
            }

            if (await _repositorioUsuario.ExisteCodigoAsync(actualizarUsuarioDto.Codigo, actualizarUsuarioDto.Id))
            {
                throw new InvalidOperationException("El código ya existe");
            }

            var usuarioExistente = await _repositorioUsuario.ObtenerPorIdAsync(actualizarUsuarioDto.Id);
            if (usuarioExistente == null)
            {
                throw new KeyNotFoundException("Usuario no encontrado");
            }

            usuarioExistente.Codigo = actualizarUsuarioDto.Codigo;
            usuarioExistente.Nombre = actualizarUsuarioDto.Nombre;
            usuarioExistente.Correo = actualizarUsuarioDto.Correo;
            usuarioExistente.IdRol = actualizarUsuarioDto.IdRol;
            usuarioExistente.Estado = actualizarUsuarioDto.Estado;

            await _repositorioUsuario.ActualizarAsync(usuarioExistente);
        }

        public async Task EliminarAsync(int id)
        {
            await _repositorioUsuario.EliminarAsync(id);
        }

        public async Task<bool> ExisteCorreoAsync(string correo, int excluirId = 0)
        {
            return await _repositorioUsuario.ExisteEmailAsync(correo, excluirId);
        }

        public async Task<bool> ExisteCodigoAsync(string codigo, int excluirId = 0)
        {
            return await _repositorioUsuario.ExisteCodigoAsync(codigo, excluirId);
        }
    }
}
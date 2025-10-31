using EG.SucursalesQuala.Application.DTOs;

namespace EG.SucursalesQuala.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioDto>> ObtenerTodosAsync();
        Task<UsuarioDto?> ObtenerPorIdAsync(int id);
        Task<UsuarioDto> CrearAsync(CrearUsuarioDto crearUsuarioDto);
        Task ActualizarAsync(ActualizarUsuarioDto actualizarUsuarioDto);
        Task EliminarAsync(int id);
        Task<bool> ExisteCorreoAsync(string correo, int excluirId = 0);
        Task<bool> ExisteCodigoAsync(string codigo, int excluirId = 0);
    }
}

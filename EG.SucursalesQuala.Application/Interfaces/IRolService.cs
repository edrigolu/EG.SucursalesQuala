using EG.SucursalesQuala.Application.DTOs;

namespace EG.SucursalesQuala.Application.Interfaces
{
    public interface IRolService
    {
        Task<IEnumerable<RolDto>> ObtenerTodosAsync();
        Task<IEnumerable<RolDto>> ObtenerActivosAsync();
        Task<RolDto?> ObtenerPorIdAsync(int id);
        Task<RolDto> CrearAsync(CrearRolDto crearRolDto);
        Task ActualizarAsync(ActualizarRolDto actualizarRolDto);
        Task EliminarAsync(int id);
        Task<bool> ExisteCodigoAsync(string codigo, int excluirId = 0);
    }
}

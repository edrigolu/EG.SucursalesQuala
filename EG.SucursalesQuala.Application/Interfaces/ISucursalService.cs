using EG.SucursalesQuala.Application.DTOs;

namespace EG.SucursalesQuala.Application.Interfaces
{
    public interface ISucursalService
    {
        Task<IEnumerable<SucursalDto>> ObtenerTodosAsync();
        Task<SucursalDto?> ObtenerPorIdAsync(int id);
        Task<SucursalDto> CrearAsync(CrearSucursalDto crearSucursalDto);
        Task ActualizarAsync(ActualizarSucursalDto actualizarSucursalDto);
        Task EliminarAsync(int id);
        Task<bool> ExisteCodigoAsync(string codigo, int excluirId = 0);
        Task<bool> ExisteIdentificacionAsync(string identificacion, int excluirId = 0);
    }
}

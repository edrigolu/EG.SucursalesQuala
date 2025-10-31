using EG.SucursalesQuala.Application.DTOs;

namespace EG.SucursalesQuala.Application.Interfaces
{
    public interface IMonedaService
    {
        Task<IEnumerable<MonedaDto>> ObtenerTodosAsync();
        Task<IEnumerable<MonedaDto>> ObtenerActivasAsync();
        Task<MonedaDto?> ObtenerPorIdAsync(int id);
        Task<MonedaDto> CrearAsync(CrearMonedaDto crearMonedaDto);
        Task ActualizarAsync(ActualizarMonedaDto actualizarMonedaDto);
        Task EliminarAsync(int id);
        Task<bool> ExisteCodigoAsync(string codigo, int excluirId = 0);
    }
}

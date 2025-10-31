using EG.SucursalesQuala.Domain.Entities;

namespace EG.SucursalesQuala.Domain.Interfaces
{
    public interface IRepositorioSucursal : IRepositorio<Sucursal>
    {
        Task<IEnumerable<Sucursal>> ObtenerConMonedaAsync();
        Task<Sucursal?> ObtenerPorIdConMonedaAsync(int id);
        Task<bool> ExisteCodigoAsync(string codigo, int excluirId = 0);
        Task<bool> ExisteIdentificacionAsync(string identificacion, int excluirId = 0);
    }
}

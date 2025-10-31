using EG.SucursalesQuala.Domain.Entities;

namespace EG.SucursalesQuala.Domain.Interfaces
{
    public interface IRepositorioRol : IRepositorio<Rol>
    {
        Task<IEnumerable<Rol>> ObtenerActivosAsync();
        Task<bool> ExisteCodigoAsync(string codigo, int excluirId = 0);
    }
}

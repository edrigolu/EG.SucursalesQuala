using EG.SucursalesQuala.Domain.Entities;

namespace EG.SucursalesQuala.Domain.Interfaces
{
    public interface IRepositorioMoneda : IRepositorio<Moneda>
    {
        Task<IEnumerable<Moneda>> ObtenerActivasAsync();
        Task<bool> ExisteCodigoAsync(string codigo, int excluirId = 0);
    }
}

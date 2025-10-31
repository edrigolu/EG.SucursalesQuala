using EG.SucursalesQuala.Domain.Entities;

namespace EG.SucursalesQuala.Domain.Interfaces
{
    public interface IRepositorioUsuario : IRepositorio<Usuario>
    {
        Task<Usuario?> ObtenerPorEmailAsync(string correo);
        Task<Usuario?> ObtenerPorEmailYClaveAsync(string correo, string clave);
        Task<bool> ExisteEmailAsync(string correo, int excluirId = 0);
        Task<bool> ExisteCodigoAsync(string codigo, int excluirId = 0);
    }
}

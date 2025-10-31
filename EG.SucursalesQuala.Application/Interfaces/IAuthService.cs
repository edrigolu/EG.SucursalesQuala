using EG.SucursalesQuala.Application.DTOs;

namespace EG.SucursalesQuala.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> AutenticarAsync(LoginRequest request);
    }
}

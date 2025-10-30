namespace EG.SucursalesQuala.Application.DTOs
{
    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public UsuarioDto Usuario { get; set; } = null!;
    }
}

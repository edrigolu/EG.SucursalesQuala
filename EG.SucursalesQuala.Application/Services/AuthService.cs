using EG.SucursalesQuala.Application.DTOs;
using EG.SucursalesQuala.Application.Interfaces;
using EG.SucursalesQuala.Domain.Entities;
using EG.SucursalesQuala.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EG.SucursalesQuala.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepositorioUsuario _repositorioUsuario;
        private readonly IEncriptacionService _encriptacionService;
        private readonly IConfiguration _configuration;

        public AuthService(
            IRepositorioUsuario repositorioUsuario,
            IEncriptacionService encriptacionService,
            IConfiguration configuration)
        {
            _repositorioUsuario = repositorioUsuario;
            _encriptacionService = encriptacionService;
            _configuration = configuration;
        }

        public async Task<LoginResponse> AutenticarAsync(LoginRequest request)
        {
            string claveEncriptada = _encriptacionService.Encriptar(request.Clave);
            Usuario? usuario = await _repositorioUsuario.ObtenerPorEmailYClaveAsync(request.Correo, claveEncriptada);

            if (usuario! == null)
            {
                throw new UnauthorizedAccessException("Credenciales inválidas");
            }

            string token = GenerarTokenJwt(usuario);

            return new LoginResponse
            {
                Token = token,
                Usuario = new UsuarioDto
                {
                    Id = usuario.Id,
                    Codigo = usuario.Codigo,
                    Nombre = usuario.Nombre,
                    Correo = usuario.Correo,
                    IdRol = usuario.IdRol,
                    NombreRol = usuario.Rol?.Nombre
                }
            };
        }

        private string GenerarTokenJwt(Usuario usuario)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim("Codigo", usuario.Codigo),
                new Claim("Rol", usuario.IdRol.ToString())
            };

            JwtSecurityToken token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
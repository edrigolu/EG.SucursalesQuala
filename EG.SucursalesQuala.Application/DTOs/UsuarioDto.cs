using EG.SucursalesQuala.Domain.Entities;

namespace EG.SucursalesQuala.Application.DTOs
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public int IdRol { get; set; }
        public string? NombreRol { get; set; }
        public bool Estado { get; set; }
    }
}
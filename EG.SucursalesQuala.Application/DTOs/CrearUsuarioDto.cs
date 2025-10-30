namespace EG.SucursalesQuala.Application.DTOs
{
    public class CrearUsuarioDto
    {
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Clave { get; set; } = null!;
        public int IdRol { get; set; }
    }
}

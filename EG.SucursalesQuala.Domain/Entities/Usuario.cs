namespace EG.SucursalesQuala.Domain.Entities
{
    public class Usuario : EntidadBase
    {
        public string Correo { get; set; } = null!;
        public string Clave { get; set; } = null!;
        public int IdRol { get; set; }
        public Rol? Rol { get; set; }
    }

}

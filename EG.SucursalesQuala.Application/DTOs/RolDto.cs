namespace EG.SucursalesQuala.Application.DTOs
{
    public class RolDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public bool Estado { get; set; }
    }
}

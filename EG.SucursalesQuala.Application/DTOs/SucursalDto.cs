namespace EG.SucursalesQuala.Application.DTOs
{
    public class SucursalDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string Identificacion { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        public int IdMoneda { get; set; }
        public string? NombreMoneda { get; set; }
        public string? SimboloMoneda { get; set; }
        public bool Estado { get; set; }
    }
}

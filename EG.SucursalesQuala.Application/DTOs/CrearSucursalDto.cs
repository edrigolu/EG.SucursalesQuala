namespace EG.SucursalesQuala.Application.DTOs
{
    public class CrearSucursalDto
    {
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string Identificacion { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        public int IdMoneda { get; set; }
    }
}

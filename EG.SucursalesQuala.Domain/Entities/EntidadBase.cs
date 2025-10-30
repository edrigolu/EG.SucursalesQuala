namespace EG.SucursalesQuala.Domain.Entities
{
    public abstract class EntidadBase
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Codigo { get; set; } = null!;
        public bool Estado { get; set; } = true;
    }


}

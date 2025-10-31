namespace EG.SucursalesQuala.Application.Interfaces
{
    public interface IEncriptacionService
    {
        string Encriptar(string texto);
        bool Verificar(string textoPlano, string textoEncriptado);
    }
}

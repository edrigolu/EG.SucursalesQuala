using System.Security.Cryptography;
using System.Text;
using EG.SucursalesQuala.Application.Interfaces;

namespace EG.SucursalesQuala.Application.Services
{
    public class EncriptacionService : IEncriptacionService
    {
        public string Encriptar(string texto)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(texto);
            byte[] hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public bool Verificar(string textoPlano, string textoEncriptado)
        {
            string textoPlanoEncriptado = Encriptar(textoPlano);
            return textoPlanoEncriptado == textoEncriptado;
        }
    }
}
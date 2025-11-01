using Dapper;
using EG.SucursalesQuala.Domain.Entities;
using EG.SucursalesQuala.Domain.Interfaces;
using EG.SucursalesQuala.Infrastructure.Data;
using System.Data;

namespace EG.SucursalesQuala.Infrastructure.Repositories
{
    public class RepositorioUsuario : RepositorioBase<Usuario>, IRepositorioUsuario
    {
        public RepositorioUsuario(DapperContext context) : base(context, "usuario") { }

        protected override DynamicParameters MapToParameters(Usuario entidad)
        {
            var parameters = new DynamicParameters();

            // Solo agregar @Id si es diferente de 0 (actualización)
            if (entidad.Id != 0)
            {
                parameters.Add("@Id", entidad.Id);
            }

            parameters.Add("Codigo", entidad.Codigo);
            parameters.Add("Nombre", entidad.Nombre);
            parameters.Add("Correo", entidad.Correo);
            parameters.Add("Clave", entidad.Clave);
            parameters.Add("IdRol", entidad.IdRol);

            return parameters;
        }

        public async Task<Usuario?> ObtenerPorEmailAsync(string correo)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Usuario>(
                "eg_sp_usuario_getbyemail",
                new { correo },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Usuario?> ObtenerPorEmailYClaveAsync(string correo, string clave)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Usuario>(
                "eg_sp_usuario_getbyemailandclave",
                new { correo, clave },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> ExisteEmailAsync(string correo, int excluirId = 0)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<int?>(
                "eg_sp_usuario_existeemail",
                new { correo, excluirId },
                commandType: CommandType.StoredProcedure);
            return result.HasValue;
        }

        public async Task<bool> ExisteCodigoAsync(string codigo, int excluirId = 0)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<int?>(
                "eg_sp_usuario_existecodigo",
                new { codigo, excluirId },
                commandType: CommandType.StoredProcedure);
            return result.HasValue;
        }
    }
}
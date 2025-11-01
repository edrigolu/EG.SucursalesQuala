using Dapper;
using EG.SucursalesQuala.Domain.Entities;
using EG.SucursalesQuala.Domain.Interfaces;
using EG.SucursalesQuala.Infrastructure.Data;
using System.Data;

namespace EG.SucursalesQuala.Infrastructure.Repositories
{
    public class RepositorioRol : RepositorioBase<Rol>, IRepositorioRol
    {
        public RepositorioRol(DapperContext context) : base(context, "rol") { }

        protected override DynamicParameters MapToParameters(Rol entidad)
        {
            var parameters = new DynamicParameters();

            // Solo agregar @Id si es diferente de 0 (actualización)
            if (entidad.Id != 0)
            {
                parameters.Add("@Id", entidad.Id);
            }
            parameters.Add("Codigo", entidad.Codigo);
            parameters.Add("Nombre", entidad.Nombre);
            return parameters;
        }

        public async Task<IEnumerable<Rol>> ObtenerActivosAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Rol>(
                "eg_sp_rol_getactive",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> ExisteCodigoAsync(string codigo, int excluirId = 0)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<int?>(
                "eg_sp_rol_existecodigo",
                new { codigo, excluirId },
                commandType: CommandType.StoredProcedure);
            return result.HasValue;
        }
    }
}
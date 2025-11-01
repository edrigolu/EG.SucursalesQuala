using Dapper;
using EG.SucursalesQuala.Domain.Entities;
using EG.SucursalesQuala.Domain.Interfaces;
using EG.SucursalesQuala.Infrastructure.Data;
using System.Data;

namespace EG.SucursalesQuala.Infrastructure.Repositories
{
    public class RepositorioMoneda : RepositorioBase<Moneda>, IRepositorioMoneda
    {
        public RepositorioMoneda(DapperContext context) : base(context, "moneda") { }

        protected override DynamicParameters MapToParameters(Moneda entidad)
        {
            var parameters = new DynamicParameters();

            // Solo agregar @Id si es diferente de 0 (actualización)
            if (entidad.Id != 0)
            {
                parameters.Add("@Id", entidad.Id);
            }

            parameters.Add("Codigo", entidad.Codigo);
            parameters.Add("Nombre", entidad.Nombre);
            parameters.Add("Simbolo", entidad.Simbolo);
            // EXCLUYE: Estado (el SP no lo espera para crear)
            // EXCLUYE: Sucursales (propiedad navegación)
            return parameters;
        }

        public async Task<IEnumerable<Moneda>> ObtenerActivasAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Moneda>(
                "eg_sp_moneda_getactive",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> ExisteCodigoAsync(string codigo, int excluirId = 0)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<int?>(
                "eg_sp_moneda_existecodigo",
                new { codigo, excluirId },
                commandType: CommandType.StoredProcedure);
            return result.HasValue;
        }
    }
}
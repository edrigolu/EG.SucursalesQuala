using Dapper;
using EG.SucursalesQuala.Domain.Entities;
using EG.SucursalesQuala.Domain.Interfaces;
using EG.SucursalesQuala.Infrastructure.Data;
using System.Data;

namespace EG.SucursalesQuala.Infrastructure.Repositories
{
    public class RepositorioSucursal : RepositorioBase<Sucursal>, IRepositorioSucursal
    {
        public RepositorioSucursal(DapperContext context) : base(context, "sucursal") { }

        protected override DynamicParameters MapToParameters(Sucursal entidad)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Codigo", entidad.Codigo);
            parameters.Add("Nombre", entidad.Nombre);
            parameters.Add("Descripcion", entidad.Descripcion);
            parameters.Add("Direccion", entidad.Direccion);
            parameters.Add("Identificacion", entidad.Identificacion);
            parameters.Add("FechaCreacion", entidad.FechaCreacion);
            parameters.Add("IdMoneda", entidad.IdMoneda);
            // EXCLUYE: Moneda (propiedad navegación)
            // EXCLUYE: Estado (el SP no lo espera para crear)
            return parameters;
        }

        public async Task<IEnumerable<Sucursal>> ObtenerConMonedaAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Sucursal>(
                "eg_sp_sucursal_getall",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Sucursal?> ObtenerPorIdConMonedaAsync(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Sucursal>(
                "eg_sp_sucursal_getbyid",
                new { id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> ExisteCodigoAsync(string codigo, int excluirId = 0)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<int?>(
                "eg_sp_sucursal_existecodigo",
                new { codigo, excluirId },
                commandType: CommandType.StoredProcedure);
            return result.HasValue;
        }

        public async Task<bool> ExisteIdentificacionAsync(string identificacion, int excluirId = 0)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<int?>(
                "eg_sp_sucursal_existeidentificacion",
                new { identificacion, excluirId },
                commandType: CommandType.StoredProcedure);
            return result.HasValue;
        }
    }
}
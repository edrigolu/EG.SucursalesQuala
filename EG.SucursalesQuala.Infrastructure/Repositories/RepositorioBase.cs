using Dapper;
using EG.SucursalesQuala.Domain.Interfaces;
using EG.SucursalesQuala.Infrastructure.Data;
using System.Data;

namespace EG.SucursalesQuala.Infrastructure.Repositories
{
    public abstract class RepositorioBase<T> : IRepositorio<T> where T : class
    {
        protected readonly DapperContext _context;
        private readonly string _prefix;
        private readonly string _entidad;

        public RepositorioBase(DapperContext context, string prefix)
        {
            _context = context;
            _entidad = typeof(T).Name.ToLower(); // usuario, sucursal, moneda
            _prefix = prefix; // usu, suc, mon
        }

        public async Task<T?> ObtenerPorIdAsync(int id)
        {
            string sp = $"eg_sp_{_entidad}_getbyid";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(sp,
                new { id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<T>> ObtenerTodosAsync()
        {
            string sp = $"eg_sp_{_entidad}_getall";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<T>(sp,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<T> CrearAsync(T entidad)
        {
            string sp = $"eg_sp_{_entidad}_create";
            using var connection = _context.CreateConnection();
            var parameters = MapToParameters(entidad);
            var result = await connection.QuerySingleAsync<int>(sp,
                parameters,
                commandType: CommandType.StoredProcedure);

            var prop = typeof(T).GetProperty("Id");
            prop?.SetValue(entidad, result);

            return entidad;
        }

        public async Task ActualizarAsync(T entidad)
        {
            string sp = $"eg_sp_{_entidad}_update";
            using var connection = _context.CreateConnection();
            var parameters = MapToParameters(entidad);
            await connection.ExecuteAsync(sp,
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task EliminarAsync(int id)
        {
            string sp = $"eg_sp_{_entidad}_delete";
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sp,
                new { id },
                commandType: CommandType.StoredProcedure);
        }

        protected abstract DynamicParameters MapToParameters(T entidad);
    }
}
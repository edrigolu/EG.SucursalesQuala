using System.Data;

namespace EG.SucursalesQuala.Infrastructure.Data
{
    public interface IDatabaseConnection
    {
        IDbConnection CreateConnection();
    }
}

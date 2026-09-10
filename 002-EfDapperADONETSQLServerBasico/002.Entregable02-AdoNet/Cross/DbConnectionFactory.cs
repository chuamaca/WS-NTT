using Microsoft.Data.SqlClient;

namespace _002.Entregable02_AdoNet.Cross;

public interface IDbConnectionFactory
{
    SqlConnection Create();
}

public sealed class DbConnectionFactory(IConfiguration configuration) : IDbConnectionFactory
{
    public SqlConnection Create()
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "No se encontró la cadena de conexión 'DefaultConnection'.");

        return new SqlConnection(connectionString);
    }
}

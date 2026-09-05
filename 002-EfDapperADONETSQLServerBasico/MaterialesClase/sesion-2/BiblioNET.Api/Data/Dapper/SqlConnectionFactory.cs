using Microsoft.Data.SqlClient;

namespace BiblioNET.Api.Data.Dapper;

public sealed class SqlConnectionFactory(IConfiguration configuration)
    : ISqlConnectionFactory
{
    public SqlConnection Create()
    {
        var connectionString =
            configuration.GetConnectionString("BiblioNET")
            ?? throw new InvalidOperationException(
                "No se encontró la cadena de conexión 'BiblioNET'.");

        return new SqlConnection(connectionString);
    }
}

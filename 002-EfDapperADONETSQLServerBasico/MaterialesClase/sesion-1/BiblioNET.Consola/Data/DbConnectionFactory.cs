using Microsoft.Data.SqlClient;

namespace BiblioNET.Consola.Data;

public static class DbConnectionFactory
{
    private const string ConnectionString =
        "Server=localhost;" +
        "Database=BiblioNETDB;" +
        "Integrated Security=True;" +
        "TrustServerCertificate=True;";

    public static SqlConnection Create()
    {
        return new SqlConnection(ConnectionString);
    }
}

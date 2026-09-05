using Microsoft.Data.SqlClient;

namespace BiblioNET.Api.Data.Dapper;

public interface ISqlConnectionFactory
{
    SqlConnection Create();
}

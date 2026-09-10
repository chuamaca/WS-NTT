using System.Data;
using _002.Entregable02_AdoNet.Cross;
using _002.Entregable02_AdoNet.Model;
using Microsoft.Data.SqlClient;

namespace _002.Entregable02_AdoNet.Data;

public interface ICategoriaDataAccess
{
    Task<IReadOnlyList<Categoria>> ListarAsync();
    Task<Categoria?> ObtenerPorIdAsync(int idCategoria);
    Task<int> InsertarAsync(CategoriaRequest request);
    Task<int> ActualizarAsync(int idCategoria, CategoriaRequest request);
    Task<int> EliminarAsync(int idCategoria);
}

public sealed class CategoriaDataAccess(IDbConnectionFactory connectionFactory) : ICategoriaDataAccess
{
    public async Task<IReadOnlyList<Categoria>> ListarAsync()
    {
        const string sql = """
            SELECT IdCategoria, Nombre, State, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, IsDeleted
            FROM dbo.Categorias
            WHERE IsDeleted = 0
            ORDER BY Nombre;
            """;

        var categorias = new List<Categoria>();

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            categorias.Add(Map(reader));
        }

        return categorias;
    }

    public async Task<Categoria?> ObtenerPorIdAsync(int idCategoria)
    {
        const string sql = """
            SELECT IdCategoria, Nombre, State, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, IsDeleted
            FROM dbo.Categorias
            WHERE IdCategoria = @IdCategoria AND IsDeleted = 0;
            """;

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@IdCategoria", SqlDbType.Int).Value = idCategoria;

        await using var reader = await command.ExecuteReaderAsync();

        return await reader.ReadAsync() ? Map(reader) : null;
    }

    public async Task<int> InsertarAsync(CategoriaRequest request)
    {
        const string sql = """
            INSERT INTO dbo.Categorias (Nombre, CreatedBy)
            VALUES (@Nombre, @CreatedBy);

            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = request.Nombre;
        command.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 100).Value = request.Usuario;

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task<int> ActualizarAsync(int idCategoria, CategoriaRequest request)
    {
        const string sql = """
            UPDATE dbo.Categorias
            SET
                Nombre = @Nombre,
                ModifiedBy = @ModifiedBy,
                ModifiedAt = GETDATE()
            WHERE IdCategoria = @IdCategoria AND IsDeleted = 0;
            """;

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@IdCategoria", SqlDbType.Int).Value = idCategoria;
        command.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = request.Nombre;
        command.Parameters.Add("@ModifiedBy", SqlDbType.NVarChar, 100).Value = request.Usuario;

        return await command.ExecuteNonQueryAsync();
    }

    public async Task<int> EliminarAsync(int idCategoria)
    {
        const string sql = """
            DELETE FROM dbo.Categorias
            WHERE IdCategoria = @IdCategoria;
            """;

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@IdCategoria", SqlDbType.Int).Value = idCategoria;

        return await command.ExecuteNonQueryAsync();
    }

    private static Categoria Map(SqlDataReader reader)
    {
        return new Categoria
        {
            IdCategoria = reader.GetInt32(reader.GetOrdinal("IdCategoria")),
            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
            State = reader.GetBoolean(reader.GetOrdinal("State"))
        };
    }
}

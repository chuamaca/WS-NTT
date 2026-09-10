using _004.Entregable04_Dapper.Cross;
using _004.Entregable04_Dapper.Model;
using Dapper;

namespace _004.Entregable04_Dapper.Data;

public sealed class ClienteDapperRepository(ISqlConnectionFactory connectionFactory) : IClienteDapperRepository
{
    private const string SelectBase = """
        SELECT
            IdCliente, Nombre, Apellido, Email, Telefono, Direccion, Documento,
            State, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, IsDeleted
        FROM dbo.Clientes
        """;

    public async Task<IReadOnlyList<Cliente>> ListarAsync()
    {
        const string sql = $"""
            {SelectBase}
            WHERE IsDeleted = 0
            ORDER BY Nombre, Apellido;
            """;

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        var result = await connection.QueryAsync<Cliente>(sql);
        return result.AsList();
    }

    public async Task<Cliente?> ObtenerPorIdAsync(int idCliente)
    {
        const string sql = $"""
            {SelectBase}
            WHERE IdCliente = @IdCliente AND IsDeleted = 0;
            """;

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        return await connection.QuerySingleOrDefaultAsync<Cliente>(sql, new { IdCliente = idCliente });
    }

    public async Task<int> InsertarAsync(ClienteRequest request)
    {
        const string sql = """
            INSERT INTO dbo.Clientes (Nombre, Apellido, Email, Telefono, Direccion, Documento, CreatedBy)
            VALUES (@Nombre, @Apellido, @Email, @Telefono, @Direccion, @Documento, @CreatedBy);

            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        return await connection.QuerySingleAsync<int>(sql, new
        {
            request.Nombre,
            request.Apellido,
            request.Email,
            request.Telefono,
            request.Direccion,
            request.Documento,
            CreatedBy = request.Usuario
        });
    }

    public async Task<int> ActualizarAsync(int idCliente, ClienteRequest request)
    {
        const string sql = """
            UPDATE dbo.Clientes
            SET
                Nombre = @Nombre,
                Apellido = @Apellido,
                Email = @Email,
                Telefono = @Telefono,
                Direccion = @Direccion,
                Documento = @Documento,
                ModifiedBy = @ModifiedBy,
                ModifiedAt = GETDATE()
            WHERE IdCliente = @IdCliente AND IsDeleted = 0;
            """;

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        return await connection.ExecuteAsync(sql, new
        {
            IdCliente = idCliente,
            request.Nombre,
            request.Apellido,
            request.Email,
            request.Telefono,
            request.Direccion,
            request.Documento,
            ModifiedBy = request.Usuario
        });
    }

    public async Task<int> EliminarAsync(int idCliente)
    {
        const string sql = """
            DELETE FROM dbo.Clientes
            WHERE IdCliente = @IdCliente;
            """;

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        return await connection.ExecuteAsync(sql, new { IdCliente = idCliente });
    }
}

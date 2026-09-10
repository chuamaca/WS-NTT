using System.Data;
using _002.Entregable02_AdoNet.Cross;
using _002.Entregable02_AdoNet.Model;
using Microsoft.Data.SqlClient;

namespace _002.Entregable02_AdoNet.Data;

public interface IClienteDataAccess
{
    Task<IReadOnlyList<Cliente>> ListarAsync();
    Task<Cliente?> ObtenerPorIdAsync(int idCliente);
    Task<int> InsertarAsync(ClienteRequest request);
    Task<int> ActualizarAsync(int idCliente, ClienteRequest request);
    Task<int> EliminarAsync(int idCliente);
}

public sealed class ClienteDataAccess(IDbConnectionFactory connectionFactory) : IClienteDataAccess
{
    private const string SelectBase = """
        SELECT
            IdCliente, Nombre, Apellido, Email, Telefono, Direccion, Documento,
            State, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, IsDeleted
        FROM dbo.Clientes
        """;

    public async Task<IReadOnlyList<Cliente>> ListarAsync()
    {
        var sql = $"""
            {SelectBase}
            WHERE IsDeleted = 0
            ORDER BY Nombre, Apellido;
            """;

        var clientes = new List<Cliente>();

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            clientes.Add(Map(reader));
        }

        return clientes;
    }

    public async Task<Cliente?> ObtenerPorIdAsync(int idCliente)
    {
        var sql = $"""
            {SelectBase}
            WHERE IdCliente = @IdCliente AND IsDeleted = 0;
            """;

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@IdCliente", SqlDbType.Int).Value = idCliente;

        await using var reader = await command.ExecuteReaderAsync();

        return await reader.ReadAsync() ? Map(reader) : null;
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

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = request.Nombre;
        command.Parameters.Add("@Apellido", SqlDbType.NVarChar, 100).Value = request.Apellido;
        command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = request.Email;
        command.Parameters.Add("@Telefono", SqlDbType.NVarChar, 20).Value = request.Telefono;
        command.Parameters.Add("@Direccion", SqlDbType.NVarChar, 200).Value = request.Direccion;
        command.Parameters.Add("@Documento", SqlDbType.NVarChar, 20).Value = request.Documento;
        command.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 100).Value = request.Usuario;

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
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

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@IdCliente", SqlDbType.Int).Value = idCliente;
        command.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = request.Nombre;
        command.Parameters.Add("@Apellido", SqlDbType.NVarChar, 100).Value = request.Apellido;
        command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = request.Email;
        command.Parameters.Add("@Telefono", SqlDbType.NVarChar, 20).Value = request.Telefono;
        command.Parameters.Add("@Direccion", SqlDbType.NVarChar, 200).Value = request.Direccion;
        command.Parameters.Add("@Documento", SqlDbType.NVarChar, 20).Value = request.Documento;
        command.Parameters.Add("@ModifiedBy", SqlDbType.NVarChar, 100).Value = request.Usuario;

        return await command.ExecuteNonQueryAsync();
    }

    public async Task<int> EliminarAsync(int idCliente)
    {
        const string sql = """
            DELETE FROM dbo.Clientes
            WHERE IdCliente = @IdCliente;
            """;

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@IdCliente", SqlDbType.Int).Value = idCliente;

        return await command.ExecuteNonQueryAsync();
    }

    private static Cliente Map(SqlDataReader reader)
    {
        return new Cliente
        {
            IdCliente = reader.GetInt32(reader.GetOrdinal("IdCliente")),
            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
            Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
            Email = reader.GetString(reader.GetOrdinal("Email")),
            Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
            Direccion = reader.GetString(reader.GetOrdinal("Direccion")),
            Documento = reader.GetString(reader.GetOrdinal("Documento")),
            State = reader.GetBoolean(reader.GetOrdinal("State")),
            CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
            CreatedBy = reader.GetString(reader.GetOrdinal("CreatedBy")),
            ModifiedAt = reader.IsDBNull(reader.GetOrdinal("ModifiedAt"))
                ? null
                : reader.GetDateTime(reader.GetOrdinal("ModifiedAt")),
            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy"))
                ? null
                : reader.GetString(reader.GetOrdinal("ModifiedBy")),
            IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
        };
    }
}

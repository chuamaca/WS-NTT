using System.Data;
using _002.Entregable02_AdoNet.Cross;
using _002.Entregable02_AdoNet.Model;
using Microsoft.Data.SqlClient;

namespace _002.Entregable02_AdoNet.Data;

public interface IOrdenDetalleDataAccess
{
    Task<IReadOnlyList<OrdenDetalle>> ListarAsync();
    Task<OrdenDetalle?> ObtenerPorIdAsync(int idOrdenDetalles);
    Task<int> InsertarAsync(OrdenDetalleRequest request);
    Task<int> ActualizarAsync(int idOrdenDetalles, OrdenDetalleRequest request);
    Task<int> EliminarAsync(int idOrdenDetalles);
}

public sealed class OrdenDetalleDataAccess(IDbConnectionFactory connectionFactory) : IOrdenDetalleDataAccess
{
    private const string SelectBase = """
        SELECT
            od.IdOrdenDetalles,
            od.IdOrden,
            od.IdProducto,
            p.Nombre AS NombreProducto,
            od.Cantidad,
            od.Precio,
            od.State,
            od.CreatedAt,
            od.CreatedBy,
            od.ModifiedAt,
            od.ModifiedBy,
            od.IsDeleted
        FROM dbo.OrdenDetalles AS od
        INNER JOIN dbo.Productos AS p
            ON p.IdProducto = od.IdProducto
        """;

    public async Task<IReadOnlyList<OrdenDetalle>> ListarAsync()
    {
        var sql = $"""
            {SelectBase}
            WHERE od.IsDeleted = 0
            ORDER BY od.IdOrden;
            """;

        var detalles = new List<OrdenDetalle>();

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            detalles.Add(Map(reader));
        }

        return detalles;
    }

    public async Task<OrdenDetalle?> ObtenerPorIdAsync(int idOrdenDetalles)
    {
        var sql = $"""
            {SelectBase}
            WHERE od.IdOrdenDetalles = @IdOrdenDetalles AND od.IsDeleted = 0;
            """;

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@IdOrdenDetalles", SqlDbType.Int).Value = idOrdenDetalles;

        await using var reader = await command.ExecuteReaderAsync();

        return await reader.ReadAsync() ? Map(reader) : null;
    }

    public async Task<int> InsertarAsync(OrdenDetalleRequest request)
    {
        const string sql = """
            INSERT INTO dbo.OrdenDetalles (IdOrden, IdProducto, Cantidad, Precio, CreatedBy)
            VALUES (@IdOrden, @IdProducto, @Cantidad, @Precio, @CreatedBy);

            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@IdOrden", SqlDbType.Int).Value = request.IdOrden;
        command.Parameters.Add("@IdProducto", SqlDbType.Int).Value = request.IdProducto;
        command.Parameters.Add("@Cantidad", SqlDbType.Int).Value = request.Cantidad;
        command.Parameters.Add("@Precio", SqlDbType.Decimal).Value = request.Precio;
        command.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 100).Value = request.Usuario;

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task<int> ActualizarAsync(int idOrdenDetalles, OrdenDetalleRequest request)
    {
        const string sql = """
            UPDATE dbo.OrdenDetalles
            SET
                IdOrden = @IdOrden,
                IdProducto = @IdProducto,
                Cantidad = @Cantidad,
                Precio = @Precio,
                ModifiedBy = @ModifiedBy,
                ModifiedAt = GETDATE()
            WHERE IdOrdenDetalles = @IdOrdenDetalles AND IsDeleted = 0;
            """;

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@IdOrdenDetalles", SqlDbType.Int).Value = idOrdenDetalles;
        command.Parameters.Add("@IdOrden", SqlDbType.Int).Value = request.IdOrden;
        command.Parameters.Add("@IdProducto", SqlDbType.Int).Value = request.IdProducto;
        command.Parameters.Add("@Cantidad", SqlDbType.Int).Value = request.Cantidad;
        command.Parameters.Add("@Precio", SqlDbType.Decimal).Value = request.Precio;
        command.Parameters.Add("@ModifiedBy", SqlDbType.NVarChar, 100).Value = request.Usuario;

        return await command.ExecuteNonQueryAsync();
    }

    public async Task<int> EliminarAsync(int idOrdenDetalles)
    {
        const string sql = """
            DELETE FROM dbo.OrdenDetalles
            WHERE IdOrdenDetalles = @IdOrdenDetalles;
            """;

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@IdOrdenDetalles", SqlDbType.Int).Value = idOrdenDetalles;

        return await command.ExecuteNonQueryAsync();
    }

    private static OrdenDetalle Map(SqlDataReader reader)
    {
        return new OrdenDetalle
        {
            IdOrdenDetalles = reader.GetInt32(reader.GetOrdinal("IdOrdenDetalles")),
            IdOrden = reader.GetInt32(reader.GetOrdinal("IdOrden")),
            IdProducto = reader.GetInt32(reader.GetOrdinal("IdProducto")),
            NombreProducto = reader.GetString(reader.GetOrdinal("NombreProducto")),
            Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
            Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
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

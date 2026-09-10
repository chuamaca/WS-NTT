using System.Data;
using _002.Entregable02_AdoNet.Cross;
using _002.Entregable02_AdoNet.Model;
using Microsoft.Data.SqlClient;

namespace _002.Entregable02_AdoNet.Data;

public interface IOrdenDataAccess
{
    Task<IReadOnlyList<Orden>> ListarAsync();
    Task<Orden?> ObtenerPorIdAsync(int idOrden);
    Task<int> InsertarConDetalleAsync(OrdenCreateRequest request);
    Task<int> ActualizarAsync(int idOrden, OrdenRequest request);
    Task<int> EliminarAsync(int idOrden);
}

public sealed class OrdenDataAccess(IDbConnectionFactory connectionFactory) : IOrdenDataAccess
{
    private const string SelectBase = """
        SELECT
            o.IdOrden,
            o.IdCliente,
            c.Nombre AS NombreCliente,
            c.Apellido AS ApellidoCliente,
            o.Serie,
            o.Comprobante,
            o.Fecha,
            o.Total,
            o.State,
            o.CreatedAt,
            o.CreatedBy,
            o.ModifiedAt,
            o.ModifiedBy,
            o.IsDeleted
        FROM dbo.Ordenes AS o
        INNER JOIN dbo.Clientes AS c
            ON c.IdCliente = o.IdCliente
        """;

    private const string SelectDetalles = """
        SELECT
            od.IdOrdenDetalles,
            od.IdProducto,
            p.Nombre AS NombreProducto,
            od.Cantidad,
            od.Precio
        FROM dbo.OrdenDetalles AS od
        INNER JOIN dbo.Productos AS p
            ON p.IdProducto = od.IdProducto
        WHERE od.IdOrden = @IdOrden AND od.IsDeleted = 0
        ORDER BY od.IdOrdenDetalles;
        """;

    public async Task<IReadOnlyList<Orden>> ListarAsync()
    {
        var sql = $"""
            {SelectBase}
            WHERE o.IsDeleted = 0
            ORDER BY o.Fecha DESC;
            """;

        var ordenes = new List<Orden>();

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            ordenes.Add(Map(reader));
        }

        return ordenes;
    }

    public async Task<Orden?> ObtenerPorIdAsync(int idOrden)
    {
        var sql = $"""
            {SelectBase}
            WHERE o.IdOrden = @IdOrden AND o.IsDeleted = 0;
            """;

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        Orden? orden = null;

        await using (var command = new SqlCommand(sql, connection))
        {
            command.Parameters.Add("@IdOrden", SqlDbType.Int).Value = idOrden;

            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                orden = Map(reader);
            }
        }

        if (orden is null)
            return null;

        var detalles = new List<OrdenDetalleLinea>();

        await using (var command = new SqlCommand(SelectDetalles, connection))
        {
            command.Parameters.Add("@IdOrden", SqlDbType.Int).Value = idOrden;

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                detalles.Add(new OrdenDetalleLinea
                {
                    IdOrdenDetalles = reader.GetInt32(reader.GetOrdinal("IdOrdenDetalles")),
                    IdProducto = reader.GetInt32(reader.GetOrdinal("IdProducto")),
                    NombreProducto = reader.GetString(reader.GetOrdinal("NombreProducto")),
                    Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                    Precio = reader.GetDecimal(reader.GetOrdinal("Precio"))
                });
            }
        }

        return new Orden
        {
            IdOrden = orden.IdOrden,
            IdCliente = orden.IdCliente,
            NombreCliente = orden.NombreCliente,
            ApellidoCliente = orden.ApellidoCliente,
            Serie = orden.Serie,
            Comprobante = orden.Comprobante,
            Fecha = orden.Fecha,
            Total = orden.Total,
            State = orden.State,
            CreatedAt = orden.CreatedAt,
            CreatedBy = orden.CreatedBy,
            ModifiedAt = orden.ModifiedAt,
            ModifiedBy = orden.ModifiedBy,
            IsDeleted = orden.IsDeleted,
            Detalles = detalles
        };
    }

    public async Task<int> InsertarConDetalleAsync(OrdenCreateRequest request)
    {
        if (request.Detalles.Count == 0)
            throw new InvalidOperationException("La orden debe contener al menos un detalle.");

        const string sqlOrden = """
            INSERT INTO dbo.Ordenes (IdCliente, Serie, Comprobante, Total, CreatedBy)
            VALUES (@IdCliente, @Serie, @Comprobante, @Total, @CreatedBy);

            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        const string sqlDetalle = """
            INSERT INTO dbo.OrdenDetalles (IdOrden, IdProducto, Cantidad, Precio, CreatedBy)
            VALUES (@IdOrden, @IdProducto, @Cantidad, @Precio, @CreatedBy);
            """;

        var total = request.Detalles.Sum(d => d.Cantidad * d.Precio);

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        await using var transaction = connection.BeginTransaction();

        try
        {
            int idOrden;

            await using (var command = new SqlCommand(sqlOrden, connection, transaction))
            {
                command.Parameters.Add("@IdCliente", SqlDbType.Int).Value = request.IdCliente;
                command.Parameters.Add("@Serie", SqlDbType.NVarChar, 50).Value = request.Serie;
                command.Parameters.Add("@Comprobante", SqlDbType.NVarChar, 50).Value = request.Comprobante;
                command.Parameters.Add("@Total", SqlDbType.Decimal).Value = total;
                command.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 100).Value = request.Usuario;

                var result = await command.ExecuteScalarAsync();
                idOrden = Convert.ToInt32(result);
            }

            foreach (var detalle in request.Detalles)
            {
                await using var command = new SqlCommand(sqlDetalle, connection, transaction);
                command.Parameters.Add("@IdOrden", SqlDbType.Int).Value = idOrden;
                command.Parameters.Add("@IdProducto", SqlDbType.Int).Value = detalle.IdProducto;
                command.Parameters.Add("@Cantidad", SqlDbType.Int).Value = detalle.Cantidad;
                command.Parameters.Add("@Precio", SqlDbType.Decimal).Value = detalle.Precio;
                command.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 100).Value = request.Usuario;

                await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();

            return idOrden;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<int> ActualizarAsync(int idOrden, OrdenRequest request)
    {
        const string sql = """
            UPDATE dbo.Ordenes
            SET
                IdCliente = @IdCliente,
                Serie = @Serie,
                Comprobante = @Comprobante,
                Total = @Total,
                ModifiedBy = @ModifiedBy,
                ModifiedAt = GETDATE()
            WHERE IdOrden = @IdOrden AND IsDeleted = 0;
            """;

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@IdOrden", SqlDbType.Int).Value = idOrden;
        command.Parameters.Add("@IdCliente", SqlDbType.Int).Value = request.IdCliente;
        command.Parameters.Add("@Serie", SqlDbType.NVarChar, 50).Value = request.Serie;
        command.Parameters.Add("@Comprobante", SqlDbType.NVarChar, 50).Value = request.Comprobante;
        command.Parameters.Add("@Total", SqlDbType.Decimal).Value = request.Total;
        command.Parameters.Add("@ModifiedBy", SqlDbType.NVarChar, 100).Value = request.Usuario;

        return await command.ExecuteNonQueryAsync();
    }

    public async Task<int> EliminarAsync(int idOrden)
    {
        const string sql = """
            DELETE FROM dbo.Ordenes
            WHERE IdOrden = @IdOrden;
            """;

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@IdOrden", SqlDbType.Int).Value = idOrden;

        return await command.ExecuteNonQueryAsync();
    }

    private static Orden Map(SqlDataReader reader)
    {
        return new Orden
        {
            IdOrden = reader.GetInt32(reader.GetOrdinal("IdOrden")),
            IdCliente = reader.GetInt32(reader.GetOrdinal("IdCliente")),
            NombreCliente = reader.GetString(reader.GetOrdinal("NombreCliente")),
            ApellidoCliente = reader.GetString(reader.GetOrdinal("ApellidoCliente")),
            Serie = reader.GetString(reader.GetOrdinal("Serie")),
            Comprobante = reader.GetString(reader.GetOrdinal("Comprobante")),
            Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
            Total = reader.GetDecimal(reader.GetOrdinal("Total")),
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

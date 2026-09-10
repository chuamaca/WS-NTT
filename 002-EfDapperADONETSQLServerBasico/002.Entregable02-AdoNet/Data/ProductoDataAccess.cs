using System.Data;
using _002.Entregable02_AdoNet.Cross;
using _002.Entregable02_AdoNet.Model;
using Microsoft.Data.SqlClient;

namespace _002.Entregable02_AdoNet.Data;

public interface IProductoDataAccess
{
    Task<IReadOnlyList<Producto>> ListarAsync();
    Task<Producto?> ObtenerPorIdAsync(int idProducto);
    Task<int> InsertarAsync(ProductoRequest request);
    Task<int> ActualizarAsync(int idProducto, ProductoRequest request);
    Task<int> EliminarAsync(int idProducto);
}

public sealed class ProductoDataAccess(IDbConnectionFactory connectionFactory) : IProductoDataAccess
{
    private const string SelectBase = """
        SELECT
            p.IdProducto,
            p.IdCategoria,
            c.Nombre AS NombreCategoria,
            p.Nombre,
            p.Precio,
            p.Stock,
            p.State,
            p.CreatedAt,
            p.CreatedBy,
            p.ModifiedAt,
            p.ModifiedBy,
            p.IsDeleted
        FROM dbo.Productos AS p
        INNER JOIN dbo.Categorias AS c
            ON c.IdCategoria = p.IdCategoria
        """;

    public async Task<IReadOnlyList<Producto>> ListarAsync()
    {
        var sql = $"""
            {SelectBase}
            WHERE p.IsDeleted = 0
            ORDER BY p.Nombre;
            """;

        var productos = new List<Producto>();

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            productos.Add(Map(reader));
        }

        return productos;
    }

    public async Task<Producto?> ObtenerPorIdAsync(int idProducto)
    {
        var sql = $"""
            {SelectBase}
            WHERE p.IdProducto = @IdProducto AND p.IsDeleted = 0;
            """;

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@IdProducto", SqlDbType.Int).Value = idProducto;

        await using var reader = await command.ExecuteReaderAsync();

        return await reader.ReadAsync() ? Map(reader) : null;
    }

    public async Task<int> InsertarAsync(ProductoRequest request)
    {
        const string sql = """
            INSERT INTO dbo.Productos (IdCategoria, Nombre, Precio, Stock, CreatedBy)
            VALUES (@IdCategoria, @Nombre, @Precio, @Stock, @CreatedBy);

            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@IdCategoria", SqlDbType.Int).Value = request.IdCategoria;
        command.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = request.Nombre;
        command.Parameters.Add("@Precio", SqlDbType.Decimal).Value = request.Precio;
        command.Parameters.Add("@Stock", SqlDbType.Int).Value = request.Stock;
        command.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 100).Value = request.Usuario;

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task<int> ActualizarAsync(int idProducto, ProductoRequest request)
    {
        const string sql = """
            UPDATE dbo.Productos
            SET
                IdCategoria = @IdCategoria,
                Nombre = @Nombre,
                Precio = @Precio,
                Stock = @Stock,
                ModifiedBy = @ModifiedBy,
                ModifiedAt = GETDATE()
            WHERE IdProducto = @IdProducto AND IsDeleted = 0;
            """;

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@IdProducto", SqlDbType.Int).Value = idProducto;
        command.Parameters.Add("@IdCategoria", SqlDbType.Int).Value = request.IdCategoria;
        command.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = request.Nombre;
        command.Parameters.Add("@Precio", SqlDbType.Decimal).Value = request.Precio;
        command.Parameters.Add("@Stock", SqlDbType.Int).Value = request.Stock;
        command.Parameters.Add("@ModifiedBy", SqlDbType.NVarChar, 100).Value = request.Usuario;

        return await command.ExecuteNonQueryAsync();
    }

    public async Task<int> EliminarAsync(int idProducto)
    {
        const string sql = """
            DELETE FROM dbo.Productos
            WHERE IdProducto = @IdProducto;
            """;

        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@IdProducto", SqlDbType.Int).Value = idProducto;

        return await command.ExecuteNonQueryAsync();
    }

    private static Producto Map(SqlDataReader reader)
    {
        return new Producto
        {
            IdProducto = reader.GetInt32(reader.GetOrdinal("IdProducto")),
            IdCategoria = reader.GetInt32(reader.GetOrdinal("IdCategoria")),
            NombreCategoria = reader.GetString(reader.GetOrdinal("NombreCategoria")),
            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
            Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
            Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
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

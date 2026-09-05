using System.Data;
using BiblioNET.Consola.Models;
using Microsoft.Data.SqlClient;

namespace BiblioNET.Consola.Data;

public sealed class LibroSqlDirectoDataAccess
{
    public async Task<IReadOnlyList<Libro>> ListarAsync()
    {
        const string sql = """
            SELECT
                l.LibroId,
                l.GeneroId,
                g.Nombre AS Genero,
                l.Titulo,
                l.Autor,
                l.ISBN,
                l.Stock,
                l.Activo
            FROM dbo.Libros AS l
            INNER JOIN dbo.Generos AS g
                ON g.GeneroId = l.GeneroId
            WHERE l.Activo = 1
            ORDER BY l.Titulo;
            """;

        var libros = new List<Libro>();

        await using var connection = DbConnectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection)
        {
            CommandType = CommandType.Text
        };

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            libros.Add(LibroReaderMapper.Map(reader));
        }

        return libros;
    }

    public async Task<Libro?> ObtenerPorIdAsync(int libroId)
    {
        const string sql = """
            SELECT
                l.LibroId,
                l.GeneroId,
                g.Nombre AS Genero,
                l.Titulo,
                l.Autor,
                l.ISBN,
                l.Stock,
                l.Activo
            FROM dbo.Libros AS l
            INNER JOIN dbo.Generos AS g
                ON g.GeneroId = l.GeneroId
            WHERE l.LibroId = @LibroId;
            """;

        await using var connection = DbConnectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@LibroId", SqlDbType.Int).Value = libroId;

        await using var reader = await command.ExecuteReaderAsync();

        return await reader.ReadAsync()
            ? LibroReaderMapper.Map(reader)
            : null;
    }

    public async Task<int> InsertarAsync(LibroInput libro)
    {
        const string sql = """
            INSERT INTO dbo.Libros (GeneroId, Titulo, Autor, ISBN, Stock)
            VALUES (@GeneroId, @Titulo, @Autor, @ISBN, @Stock);

            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        await using var connection = DbConnectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        
        command.Parameters.Add("@GeneroId", SqlDbType.Int).Value = libro.GeneroId;
        command.Parameters.Add("@Titulo", SqlDbType.NVarChar, 150).Value = libro.Titulo;
        command.Parameters.Add("@Autor", SqlDbType.NVarChar, 120).Value = libro.Autor;
        command.Parameters.Add("@ISBN", SqlDbType.VarChar, 20).Value = libro.ISBN;
        command.Parameters.Add("@Stock", SqlDbType.Int).Value = libro.Stock;

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task<int> ActualizarAsync(int libroId, LibroInput libro)
    {
        const string sql = """
            UPDATE dbo.Libros
            SET
                GeneroId = @GeneroId,
                Titulo = @Titulo,
                Autor = @Autor,
                ISBN = @ISBN,
                Stock = @Stock
            WHERE LibroId = @LibroId;
            """;

        await using var connection = DbConnectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@LibroId", SqlDbType.Int).Value = libroId;        
        command.Parameters.Add("@GeneroId", SqlDbType.Int).Value = libro.GeneroId;
        command.Parameters.Add("@Titulo", SqlDbType.NVarChar, 150).Value = libro.Titulo;
        command.Parameters.Add("@Autor", SqlDbType.NVarChar, 120).Value = libro.Autor;
        command.Parameters.Add("@ISBN", SqlDbType.VarChar, 20).Value = libro.ISBN;
        command.Parameters.Add("@Stock", SqlDbType.Int).Value = libro.Stock;

        return await command.ExecuteNonQueryAsync();
    }

    public async Task<int> EliminarAsync(int libroId)
    {
        const string sql = """
            DELETE FROM dbo.Libros
            WHERE LibroId = @LibroId;
            """;

        await using var connection = DbConnectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@LibroId", SqlDbType.Int).Value = libroId;

        return await command.ExecuteNonQueryAsync();
    }
}

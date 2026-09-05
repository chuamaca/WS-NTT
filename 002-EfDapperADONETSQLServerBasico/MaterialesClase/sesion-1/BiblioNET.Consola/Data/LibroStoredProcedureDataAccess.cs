using System.Data;
using BiblioNET.Consola.Models;
using Microsoft.Data.SqlClient;

namespace BiblioNET.Consola.Data;

public sealed class LibroStoredProcedureDataAccess
{
    public async Task<IReadOnlyList<Libro>> ListarAsync()
    {
        var libros = new List<Libro>();

        await using var connection = DbConnectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand("dbo.usp_Libro_Listar", connection)
        {
            CommandType = CommandType.StoredProcedure
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
        await using var connection = DbConnectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand("dbo.usp_Libro_ObtenerPorId", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.Add("@LibroId", SqlDbType.Int).Value = libroId;

        await using var reader = await command.ExecuteReaderAsync();

        return await reader.ReadAsync()
            ? LibroReaderMapper.Map(reader)
            : null;
    }

    public async Task<int> InsertarAsync(LibroInput libro)
    {
        await using var connection = DbConnectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand("dbo.usp_Libro_Insertar", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

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
        await using var connection = DbConnectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand("dbo.usp_Libro_Actualizar", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.Add("@LibroId", SqlDbType.Int).Value = libroId;
        command.Parameters.Add("@GeneroId", SqlDbType.Int).Value = libro.GeneroId;
        command.Parameters.Add("@Titulo", SqlDbType.NVarChar, 150).Value = libro.Titulo;
        command.Parameters.Add("@Autor", SqlDbType.NVarChar, 120).Value = libro.Autor;
        command.Parameters.Add("@ISBN", SqlDbType.VarChar, 20).Value = libro.ISBN;
        command.Parameters.Add("@Stock", SqlDbType.Int).Value = libro.Stock;

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task<int> EliminarAsync(int libroId)
    {
        await using var connection = DbConnectionFactory.Create();
        await connection.OpenAsync();

        await using var command = new SqlCommand("dbo.usp_Libro_Eliminar", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.Add("@LibroId", SqlDbType.Int).Value = libroId;

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }
}

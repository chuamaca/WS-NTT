using System.Data;
using BiblioNET.Api.Dtos;
using Dapper;

namespace BiblioNET.Api.Data.Dapper;

public sealed class LibroDapperRepository(
    ISqlConnectionFactory connectionFactory)
    : ILibroDapperRepository
{
    public async Task<IReadOnlyList<LibroDto>> ListarAsync()
    {
        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        var result = await connection.QueryAsync<LibroDto>(
            "dbo.usp_Libro_Listar",
            commandType: CommandType.StoredProcedure);

        return result.AsList();
    }

    public async Task<LibroDto?> ObtenerPorIdAsync(int libroId)
    {
        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        return await connection.QuerySingleOrDefaultAsync<LibroDto>(
            "dbo.usp_Libro_ObtenerPorId",
            new { LibroId = libroId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> InsertarAsync(LibroRequest request)
    {
        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        return await connection.QuerySingleAsync<int>(
            "dbo.usp_Libro_Insertar",
            new
            {
                request.GeneroId,
                request.Titulo,
                request.Autor,
                request.ISBN,
                request.Stock
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> ActualizarAsync(int libroId, LibroRequest request)
    {
        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        return await connection.QuerySingleAsync<int>(
            "dbo.usp_Libro_Actualizar",
            new
            {
                LibroId = libroId,
                request.GeneroId,
                request.Titulo,
                request.Autor,
                request.ISBN,
                request.Stock
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> EliminarAsync(int libroId)
    {
        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        return await connection.QuerySingleAsync<int>(
            "dbo.usp_Libro_Eliminar",
            new { LibroId = libroId },
            commandType: CommandType.StoredProcedure);
    }
}

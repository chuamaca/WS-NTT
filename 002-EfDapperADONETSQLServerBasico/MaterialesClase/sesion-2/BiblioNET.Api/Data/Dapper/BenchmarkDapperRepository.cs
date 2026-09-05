using System.Data;
using BiblioNET.Api.Dtos;
using Dapper;

namespace BiblioNET.Api.Data.Dapper;

public sealed class BenchmarkDapperRepository(
    ISqlConnectionFactory connectionFactory)
    : IBenchmarkDapperRepository
{
    public async Task<long> ContarSinIndiceAsync(
        int generoId,
        int stock)
    {
        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        return await connection.QuerySingleAsync<long>(
            "dbo.usp_BenchmarkLibros_SinIndice_Contar",
            new
            {
                GeneroId = generoId,
                Stock = stock
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<long> ContarConIndiceAsync(
        int generoId,
        int stock)
    {
        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        return await connection.QuerySingleAsync<long>(
            "dbo.usp_BenchmarkLibros_ConIndice_Contar",
            new
            {
                GeneroId = generoId,
                Stock = stock
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IReadOnlyList<BenchmarkLibroDto>>
        ConsultarSinIndiceAsync(
            int generoId,
            int stock)
    {
        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        var result = await connection.QueryAsync<BenchmarkLibroDto>(
            "dbo.usp_BenchmarkLibros_SinIndice_Consultar",
            new
            {
                GeneroId = generoId,
                Stock = stock
            },
            commandType: CommandType.StoredProcedure);

        return result.AsList();
    }

    public async Task<IReadOnlyList<BenchmarkLibroDto>>
        ConsultarConIndiceAsync(
            int generoId,
            int stock)
    {
        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        var result = await connection.QueryAsync<BenchmarkLibroDto>(
            "dbo.usp_BenchmarkLibros_ConIndice_Consultar",
            new
            {
                GeneroId = generoId,
                Stock = stock
            },
            commandType: CommandType.StoredProcedure);

        return result.AsList();
    }
}
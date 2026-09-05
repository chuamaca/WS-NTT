using BiblioNET.Api.Data;
using BiblioNET.Api.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BiblioNET.Api.Services;

public sealed class BenchmarkEfService(
    BiblioNetDbContext db)
{
    public async Task<long> ContarSinIndiceAsync(
        int generoId,
        int stock)
    {
        var generoParameter =
            new SqlParameter("@GeneroId", generoId);

        var stockParameter =
            new SqlParameter("@Stock", stock);

        var resultado = await db.Database
            .SqlQueryRaw<long>(
                """
                EXEC dbo.usp_BenchmarkLibros_SinIndice_Contar
                    @GeneroId,
                    @Stock
                """,
                generoParameter,
                stockParameter)
            .ToListAsync();

        return resultado.Single();
    }

    public async Task<long> ContarConIndiceAsync(
        int generoId,
        int stock)
    {
        var generoParameter =
            new SqlParameter("@GeneroId", generoId);

        var stockParameter =
            new SqlParameter("@Stock", stock);

        var resultado = await db.Database
            .SqlQueryRaw<long>(
                """
                EXEC dbo.usp_BenchmarkLibros_ConIndice_Contar
                    @GeneroId,
                    @Stock
                """,
                generoParameter,
                stockParameter)
            .ToListAsync();

        return resultado.Single();
    }

    public async Task<IReadOnlyList<BenchmarkLibroDto>>
        ConsultarSinIndiceAsync(
            int generoId,
            int stock)
    {
        var generoParameter =
            new SqlParameter("@GeneroId", generoId);

        var stockParameter =
            new SqlParameter("@Stock", stock);

        return await db.Database
            .SqlQueryRaw<BenchmarkLibroDto>(
                """
                EXEC dbo.usp_BenchmarkLibros_SinIndice_Consultar
                    @GeneroId,
                    @Stock
                """,
                generoParameter,
                stockParameter)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<BenchmarkLibroDto>>
        ConsultarConIndiceAsync(
            int generoId,
            int stock)
    {
        var generoParameter =
            new SqlParameter("@GeneroId", generoId);

        var stockParameter =
            new SqlParameter("@Stock", stock);

        return await db.Database
            .SqlQueryRaw<BenchmarkLibroDto>(
                """
                EXEC dbo.usp_BenchmarkLibros_ConIndice_Consultar
                    @GeneroId,
                    @Stock
                """,
                generoParameter,
                stockParameter)
            .ToListAsync();
    }
}
using BiblioNET.Api.Data.Dapper;
using BiblioNET.Api.Services;

namespace BiblioNET.Api.Endpoints;

public static class BenchmarkEndpoints
{
    public static RouteGroupBuilder MapBenchmarkEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/benchmark")
            .WithTags("Benchmark didáctico");

        MapCountEndpoints(group);
        MapQueryEndpoints(group);

        return group;
    }

    private static void MapCountEndpoints(
        RouteGroupBuilder group)
    {
        group.MapGet("/count/ef/sin-indice", async (
            int generoId,
            int stock,
            int repeticiones,
            BenchmarkEfService service,
            BenchmarkRunner runner) =>
            Results.Ok(
                await runner.EjecutarConteoAsync(
                    "EF Core",
                    "COUNT sin índice secundario",
                    repeticiones,
                    () => service.ContarSinIndiceAsync(
                        generoId,
                        stock))));

        group.MapGet("/count/ef/con-indice", async (
            int generoId,
            int stock,
            int repeticiones,
            BenchmarkEfService service,
            BenchmarkRunner runner) =>
            Results.Ok(
                await runner.EjecutarConteoAsync(
                    "EF Core",
                    "COUNT con índice",
                    repeticiones,
                    () => service.ContarConIndiceAsync(
                        generoId,
                        stock))));

        group.MapGet("/count/dapper/sin-indice", async (
            int generoId,
            int stock,
            int repeticiones,
            IBenchmarkDapperRepository repo,
            BenchmarkRunner runner) =>
            Results.Ok(
                await runner.EjecutarConteoAsync(
                    "Dapper",
                    "COUNT sin índice secundario",
                    repeticiones,
                    () => repo.ContarSinIndiceAsync(
                        generoId,
                        stock))));

        group.MapGet("/count/dapper/con-indice", async (
            int generoId,
            int stock,
            int repeticiones,
            IBenchmarkDapperRepository repo,
            BenchmarkRunner runner) =>
            Results.Ok(
                await runner.EjecutarConteoAsync(
                    "Dapper",
                    "COUNT con índice",
                    repeticiones,
                    () => repo.ContarConIndiceAsync(
                        generoId,
                        stock))));
    }

    private static void MapQueryEndpoints(
        RouteGroupBuilder group)
    {
        group.MapGet("/query/ef/sin-indice", async (
            int generoId,
            int stock,
            int repeticiones,
            BenchmarkEfService service,
            BenchmarkRunner runner) =>
            Results.Ok(
                await runner.EjecutarAsync(
                    "EF Core",
                    "TOP 100 sin índice",
                    repeticiones,
                    () => service.ConsultarSinIndiceAsync(
                        generoId,
                        stock))));

        group.MapGet("/query/ef/con-indice", async (
            int generoId,
            int stock,
            int repeticiones,
            BenchmarkEfService service,
            BenchmarkRunner runner) =>
            Results.Ok(
                await runner.EjecutarAsync(
                    "EF Core",
                    "TOP 100 con índice",
                    repeticiones,
                    () => service.ConsultarConIndiceAsync(
                        generoId,
                        stock))));

        group.MapGet("/query/dapper/sin-indice", async (
            int generoId,
            int stock,
            int repeticiones,
            IBenchmarkDapperRepository repo,
            BenchmarkRunner runner) =>
            Results.Ok(
                await runner.EjecutarAsync(
                    "Dapper",
                    "TOP 100 sin índice",
                    repeticiones,
                    () => repo.ConsultarSinIndiceAsync(
                        generoId,
                        stock))));

        group.MapGet("/query/dapper/con-indice", async (
            int generoId,
            int stock,
            int repeticiones,
            IBenchmarkDapperRepository repo,
            BenchmarkRunner runner) =>
            Results.Ok(
                await runner.EjecutarAsync(
                    "Dapper",
                    "TOP 100 con índice",
                    repeticiones,
                    () => repo.ConsultarConIndiceAsync(
                        generoId,
                        stock))));
    }
}
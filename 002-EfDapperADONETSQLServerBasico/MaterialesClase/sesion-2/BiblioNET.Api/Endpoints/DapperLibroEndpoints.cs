using BiblioNET.Api.Data.Dapper;
using BiblioNET.Api.Dtos;

namespace BiblioNET.Api.Endpoints;

public static class DapperLibroEndpoints
{
    public static RouteGroupBuilder MapDapperLibroEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/dapper/libros")
            .WithTags("Dapper");

        group.MapGet("/", async (ILibroDapperRepository repo) =>
            Results.Ok(await repo.ListarAsync()));

        group.MapGet("/{id:int}", async (
            int id,
            ILibroDapperRepository repo) =>
        {
            var libro = await repo.ObtenerPorIdAsync(id);

            return libro is null
                ? Results.NotFound()
                : Results.Ok(libro);
        });

        group.MapPost("/", async (
            LibroRequest request,
            ILibroDapperRepository repo) =>
        {
            var id = await repo.InsertarAsync(request);

            return Results.Created(
                $"/api/dapper/libros/{id}",
                new { LibroId = id });
        });

        group.MapPut("/{id:int}", async (
            int id,
            LibroRequest request,
            ILibroDapperRepository repo) =>
        {
            var filas = await repo.ActualizarAsync(id, request);

            return filas == 0
                ? Results.NotFound()
                : Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (
            int id,
            ILibroDapperRepository repo) =>
        {
            var filas = await repo.EliminarAsync(id);

            return filas == 0
                ? Results.NotFound()
                : Results.NoContent();
        });

        return group;
    }
}

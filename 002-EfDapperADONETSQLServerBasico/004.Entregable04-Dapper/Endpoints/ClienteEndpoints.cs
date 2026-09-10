using _004.Entregable04_Dapper.Data;
using _004.Entregable04_Dapper.Model;

namespace _004.Entregable04_Dapper.Endpoints;

public static class ClienteEndpoints
{
    public static RouteGroupBuilder MapClienteEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/clientes").WithTags("Clientes (Dapper)");

        group.MapGet("/", async (IClienteDapperRepository repo) =>
            Results.Ok(await repo.ListarAsync()));

        group.MapGet("/{id:int}", async (int id, IClienteDapperRepository repo) =>
        {
            var cliente = await repo.ObtenerPorIdAsync(id);
            return cliente is null ? Results.NotFound() : Results.Ok(cliente);
        });

        group.MapPost("/", async (ClienteRequest request, IClienteDapperRepository repo) =>
        {
            var id = await repo.InsertarAsync(request);
            return Results.Created($"/api/clientes/{id}", new { IdCliente = id });
        });

        group.MapPut("/{id:int}", async (int id, ClienteRequest request, IClienteDapperRepository repo) =>
        {
            var filas = await repo.ActualizarAsync(id, request);
            return filas == 0 ? Results.NotFound() : Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (int id, IClienteDapperRepository repo) =>
        {
            var filas = await repo.EliminarAsync(id);
            return filas == 0 ? Results.NotFound() : Results.NoContent();
        });

        return group;
    }
}

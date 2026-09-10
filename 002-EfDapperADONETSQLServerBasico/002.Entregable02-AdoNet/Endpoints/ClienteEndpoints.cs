using _002.Entregable02_AdoNet.Data;
using _002.Entregable02_AdoNet.Model;

namespace _002.Entregable02_AdoNet.Endpoints;

public static class ClienteEndpoints
{
    public static RouteGroupBuilder MapClienteEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/clientes").WithTags("Clientes");

        group.MapGet("/", async (IClienteDataAccess data) =>
            Results.Ok(await data.ListarAsync()));

        group.MapGet("/{id:int}", async (int id, IClienteDataAccess data) =>
        {
            var cliente = await data.ObtenerPorIdAsync(id);
            return cliente is null ? Results.NotFound() : Results.Ok(cliente);
        });

        group.MapPost("/", async (ClienteRequest request, IClienteDataAccess data) =>
        {
            var id = await data.InsertarAsync(request);
            return Results.Created($"/api/clientes/{id}", new { IdCliente = id });
        });

        group.MapPut("/{id:int}", async (int id, ClienteRequest request, IClienteDataAccess data) =>
        {
            var filas = await data.ActualizarAsync(id, request);
            return filas == 0 ? Results.NotFound() : Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (int id, IClienteDataAccess data) =>
        {
            var filas = await data.EliminarAsync(id);
            return filas == 0 ? Results.NotFound() : Results.NoContent();
        });

        return group;
    }
}

using _002.Entregable02_AdoNet.Data;
using _002.Entregable02_AdoNet.Model;

namespace _002.Entregable02_AdoNet.Endpoints;

public static class OrdenEndpoints
{
    public static RouteGroupBuilder MapOrdenEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/ordenes").WithTags("Ordenes");

        group.MapGet("/", async (IOrdenDataAccess data) =>
            Results.Ok(await data.ListarAsync()));

        group.MapGet("/{id:int}", async (int id, IOrdenDataAccess data) =>
        {
            var orden = await data.ObtenerPorIdAsync(id);
            return orden is null ? Results.NotFound() : Results.Ok(orden);
        });

        group.MapPost("/", async (OrdenCreateRequest request, IOrdenDataAccess data) =>
        {
            var id = await data.InsertarConDetalleAsync(request);
            return Results.Created($"/api/ordenes/{id}", new { IdOrden = id });
        });

        group.MapPut("/{id:int}", async (int id, OrdenRequest request, IOrdenDataAccess data) =>
        {
            var filas = await data.ActualizarAsync(id, request);
            return filas == 0 ? Results.NotFound() : Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (int id, IOrdenDataAccess data) =>
        {
            var filas = await data.EliminarAsync(id);
            return filas == 0 ? Results.NotFound() : Results.NoContent();
        });

        return group;
    }
}

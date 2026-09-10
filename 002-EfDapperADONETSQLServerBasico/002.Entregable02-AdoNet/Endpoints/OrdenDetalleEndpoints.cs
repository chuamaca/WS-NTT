using _002.Entregable02_AdoNet.Data;
using _002.Entregable02_AdoNet.Model;

namespace _002.Entregable02_AdoNet.Endpoints;

public static class OrdenDetalleEndpoints
{
    public static RouteGroupBuilder MapOrdenDetalleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/ordendetalles").WithTags("OrdenDetalles");

        group.MapGet("/", async (IOrdenDetalleDataAccess data) =>
            Results.Ok(await data.ListarAsync()));

        group.MapGet("/{id:int}", async (int id, IOrdenDetalleDataAccess data) =>
        {
            var detalle = await data.ObtenerPorIdAsync(id);
            return detalle is null ? Results.NotFound() : Results.Ok(detalle);
        });

        group.MapPost("/", async (OrdenDetalleRequest request, IOrdenDetalleDataAccess data) =>
        {
            var id = await data.InsertarAsync(request);
            return Results.Created($"/api/ordendetalles/{id}", new { IdOrdenDetalles = id });
        });

        group.MapPut("/{id:int}", async (int id, OrdenDetalleRequest request, IOrdenDetalleDataAccess data) =>
        {
            var filas = await data.ActualizarAsync(id, request);
            return filas == 0 ? Results.NotFound() : Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (int id, IOrdenDetalleDataAccess data) =>
        {
            var filas = await data.EliminarAsync(id);
            return filas == 0 ? Results.NotFound() : Results.NoContent();
        });

        return group;
    }
}

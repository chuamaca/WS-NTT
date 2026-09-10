using _002.Entregable02_AdoNet.Data;
using _002.Entregable02_AdoNet.Model;

namespace _002.Entregable02_AdoNet.Endpoints;

public static class ProductoEndpoints
{
    public static RouteGroupBuilder MapProductoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/productos").WithTags("Productos");

        group.MapGet("/", async (IProductoDataAccess data) =>
            Results.Ok(await data.ListarAsync()));

        group.MapGet("/{id:int}", async (int id, IProductoDataAccess data) =>
        {
            var producto = await data.ObtenerPorIdAsync(id);
            return producto is null ? Results.NotFound() : Results.Ok(producto);
        });

        group.MapPost("/", async (ProductoRequest request, IProductoDataAccess data) =>
        {
            var id = await data.InsertarAsync(request);
            return Results.Created($"/api/productos/{id}", new { IdProducto = id });
        });

        group.MapPut("/{id:int}", async (int id, ProductoRequest request, IProductoDataAccess data) =>
        {
            var filas = await data.ActualizarAsync(id, request);
            return filas == 0 ? Results.NotFound() : Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (int id, IProductoDataAccess data) =>
        {
            var filas = await data.EliminarAsync(id);
            return filas == 0 ? Results.NotFound() : Results.NoContent();
        });

        return group;
    }
}

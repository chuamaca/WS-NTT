using _002.Entregable02_AdoNet.Data;
using _002.Entregable02_AdoNet.Model;

namespace _002.Entregable02_AdoNet.Endpoints;

public static class CategoriaEndpoints
{
    public static RouteGroupBuilder MapCategoriaEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/categorias").WithTags("Categorias");

        group.MapGet("/", async (ICategoriaDataAccess data) =>
            Results.Ok(await data.ListarAsync()));

        group.MapGet("/{id:int}", async (int id, ICategoriaDataAccess data) =>
        {
            var categoria = await data.ObtenerPorIdAsync(id);
            return categoria is null ? Results.NotFound() : Results.Ok(categoria);
        });

        group.MapPost("/", async (CategoriaRequest request, ICategoriaDataAccess data) =>
        {
            var id = await data.InsertarAsync(request);
            return Results.Created($"/api/categorias/{id}", new { IdCategoria = id });
        });

        group.MapPut("/{id:int}", async (int id, CategoriaRequest request, ICategoriaDataAccess data) =>
        {
            var filas = await data.ActualizarAsync(id, request);
            return filas == 0 ? Results.NotFound() : Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (int id, ICategoriaDataAccess data) =>
        {
            var filas = await data.EliminarAsync(id);
            return filas == 0 ? Results.NotFound() : Results.NoContent();
        });

        return group;
    }
}

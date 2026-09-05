using BiblioNET.Api.Data.Dapper;
using BiblioNET.Api.Dtos;

namespace BiblioNET.Api.Endpoints;

public static class DapperPrestamoEndpoints
{
    public static RouteGroupBuilder MapDapperPrestamoEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/dapper/prestamos")
            .WithTags("Dapper - Transacción");

        group.MapPost("/", async (
            CrearPrestamoRequest request,
            PrestamoDapperRepository repo) =>
        {
            try
            {
                var id = await repo.CrearAsync(request);

                return Results.Created(
                    $"/api/dapper/prestamos/{id}",
                    new { PrestamoId = id });
            }
            catch (InvalidOperationException ex)
            {
                return Results.Conflict(ex.Message);
            }
        });

        return group;
    }
}

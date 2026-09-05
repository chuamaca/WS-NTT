using BiblioNET.Api.Data;
using BiblioNET.Api.Domain;
using BiblioNET.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BiblioNET.Api.Endpoints;

public static class EfPrestamoEndpoints
{
    public static RouteGroupBuilder MapEfPrestamoEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/ef/prestamos")
            .WithTags("EF Core - Relaciones y SaveChanges");

        group.MapPost("/", CrearAsync);

        return group;
    }

    private static async Task<IResult> CrearAsync(
        CrearPrestamoRequest request,
        BiblioNetDbContext db)
    {
        var lector = await db.Lectores
            .SingleOrDefaultAsync(
                x => x.LectorId == request.LectorId && x.Activo);

        var libro = await db.Libros
            .SingleOrDefaultAsync(
                x => x.LibroId == request.LibroId && x.Activo);

        if (lector is null || libro is null)
            return Results.BadRequest("Lector o libro inexistente.");

        if (request.Cantidad <= 0 || libro.Stock < request.Cantidad)
            return Results.Conflict("Cantidad o stock inválido.");

        var fecha = DateTime.Today;

        var prestamo = new Prestamo
        {
            LectorId = lector.LectorId,
            FechaPrestamo = fecha,
            FechaVencimiento = fecha.AddDays(request.DiasPrestamo),
            Estado = "ACTIVO",
            Detalles =
            {
                new PrestamoDetalle
                {
                    LibroId = libro.LibroId,
                    Cantidad = request.Cantidad,
                    Devuelto = false
                }
            }
        };

        libro.Stock -= request.Cantidad;
        db.Prestamos.Add(prestamo);

        await db.SaveChangesAsync();

        return Results.Created(
            $"/api/ef/prestamos/{prestamo.PrestamoId}",
            new { prestamo.PrestamoId });
    }
}

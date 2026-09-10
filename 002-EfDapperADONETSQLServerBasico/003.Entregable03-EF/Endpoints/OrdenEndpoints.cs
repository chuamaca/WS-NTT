using _003.Entregable03_EF.Data;
using _003.Entregable03_EF.Domain;
using _003.Entregable03_EF.Dtos;
using Microsoft.EntityFrameworkCore;

namespace _003.Entregable03_EF.Endpoints;

public static class OrdenEndpoints
{
    public static RouteGroupBuilder MapOrdenEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/ordenes").WithTags("Ordenes");

        group.MapGet("/", ListarAsync);
        group.MapGet("/{id:int}", ObtenerAsync);
        group.MapPost("/", InsertarConDetalleAsync);
        group.MapPut("/{id:int}", ActualizarAsync);
        group.MapDelete("/{id:int}", EliminarAsync);

        return group;
    }

    private static async Task<IResult> ListarAsync(AppDbContext db)
    {
        var result = await db.Ordenes
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.Fecha)
            .Select(x => new OrdenDto(
                x.IdOrden, x.IdCliente, x.Cliente.Nombre, x.Cliente.Apellido,
                x.Serie, x.Comprobante, x.Fecha, x.Total,
                x.State, x.CreatedAt, x.CreatedBy, x.ModifiedAt, x.ModifiedBy))
            .ToListAsync();

        return Results.Ok(result);
    }

    private static async Task<IResult> ObtenerAsync(int id, AppDbContext db)
    {
        var result = await db.Ordenes
            .AsNoTracking()
            .Where(x => x.IdOrden == id && !x.IsDeleted)
            .Select(x => new OrdenConDetalleDto(
                x.IdOrden, x.IdCliente, x.Cliente.Nombre, x.Cliente.Apellido,
                x.Serie, x.Comprobante, x.Fecha, x.Total,
                x.State, x.CreatedAt, x.CreatedBy, x.ModifiedAt, x.ModifiedBy,
                x.OrdenDetalles
                    .Where(d => !d.IsDeleted)
                    .Select(d => new OrdenDetalleLineaDto(
                        d.IdOrdenDetalles, d.IdProducto, d.Producto.Nombre, d.Cantidad, d.Precio))
                    .ToList()))
            .SingleOrDefaultAsync();

        return result is null ? Results.NotFound() : Results.Ok(result);
    }

    private static async Task<IResult> InsertarConDetalleAsync(OrdenCreateRequest request, AppDbContext db)
    {
        if (request.Detalles.Count == 0)
            return Results.BadRequest("La orden debe contener al menos un detalle.");

        var total = request.Detalles.Sum(d => d.Cantidad * d.Precio);

        var orden = new Orden
        {
            IdCliente = request.IdCliente,
            Serie = request.Serie.Trim(),
            Comprobante = request.Comprobante.Trim(),
            Total = total,
            Fecha = DateTime.Now,
            CreatedBy = request.Usuario,
            CreatedAt = DateTime.Now
        };

        foreach (var detalle in request.Detalles)
        {
            orden.OrdenDetalles.Add(new OrdenDetalle
            {
                IdProducto = detalle.IdProducto,
                Cantidad = detalle.Cantidad,
                Precio = detalle.Precio,
                CreatedBy = request.Usuario,
                CreatedAt = DateTime.Now
            });
        }

        db.Ordenes.Add(orden);
        await db.SaveChangesAsync();

        return Results.Created($"/api/ordenes/{orden.IdOrden}", new { orden.IdOrden });
    }

    private static async Task<IResult> ActualizarAsync(int id, OrdenRequest request, AppDbContext db)
    {
        var orden = await db.Ordenes.SingleOrDefaultAsync(x => x.IdOrden == id && !x.IsDeleted);

        if (orden is null)
            return Results.NotFound();

        orden.IdCliente = request.IdCliente;
        orden.Serie = request.Serie.Trim();
        orden.Comprobante = request.Comprobante.Trim();
        orden.Total = request.Total;
        orden.ModifiedBy = request.Usuario;
        orden.ModifiedAt = DateTime.Now;

        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    private static async Task<IResult> EliminarAsync(int id, AppDbContext db)
    {
        var orden = await db.Ordenes.SingleOrDefaultAsync(x => x.IdOrden == id);

        if (orden is null)
            return Results.NotFound();

        db.Ordenes.Remove(orden);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }
}

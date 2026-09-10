using _003.Entregable03_EF.Data;
using _003.Entregable03_EF.Domain;
using _003.Entregable03_EF.Dtos;
using Microsoft.EntityFrameworkCore;

namespace _003.Entregable03_EF.Endpoints;

public static class OrdenDetalleEndpoints
{
    public static RouteGroupBuilder MapOrdenDetalleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/ordendetalles").WithTags("OrdenDetalles");

        group.MapGet("/", ListarAsync);
        group.MapGet("/{id:int}", ObtenerAsync);
        group.MapPost("/", InsertarAsync);
        group.MapPut("/{id:int}", ActualizarAsync);
        group.MapDelete("/{id:int}", EliminarAsync);

        return group;
    }

    private static async Task<IResult> ListarAsync(AppDbContext db)
    {
        var result = await db.OrdenDetalles
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.IdOrden)
            .Select(x => new OrdenDetalleDto(
                x.IdOrdenDetalles, x.IdOrden, x.IdProducto, x.Producto.Nombre, x.Cantidad, x.Precio,
                x.State, x.CreatedAt, x.CreatedBy, x.ModifiedAt, x.ModifiedBy))
            .ToListAsync();

        return Results.Ok(result);
    }

    private static async Task<IResult> ObtenerAsync(int id, AppDbContext db)
    {
        var result = await db.OrdenDetalles
            .AsNoTracking()
            .Where(x => x.IdOrdenDetalles == id && !x.IsDeleted)
            .Select(x => new OrdenDetalleDto(
                x.IdOrdenDetalles, x.IdOrden, x.IdProducto, x.Producto.Nombre, x.Cantidad, x.Precio,
                x.State, x.CreatedAt, x.CreatedBy, x.ModifiedAt, x.ModifiedBy))
            .SingleOrDefaultAsync();

        return result is null ? Results.NotFound() : Results.Ok(result);
    }

    private static async Task<IResult> InsertarAsync(OrdenDetalleRequest request, AppDbContext db)
    {
        var detalle = new OrdenDetalle
        {
            IdOrden = request.IdOrden,
            IdProducto = request.IdProducto,
            Cantidad = request.Cantidad,
            Precio = request.Precio,
            CreatedBy = request.Usuario,
            CreatedAt = DateTime.Now
        };

        db.OrdenDetalles.Add(detalle);
        await db.SaveChangesAsync();

        return Results.Created($"/api/ordendetalles/{detalle.IdOrdenDetalles}", new { detalle.IdOrdenDetalles });
    }

    private static async Task<IResult> ActualizarAsync(int id, OrdenDetalleRequest request, AppDbContext db)
    {
        var detalle = await db.OrdenDetalles.SingleOrDefaultAsync(x => x.IdOrdenDetalles == id && !x.IsDeleted);

        if (detalle is null)
            return Results.NotFound();

        detalle.IdOrden = request.IdOrden;
        detalle.IdProducto = request.IdProducto;
        detalle.Cantidad = request.Cantidad;
        detalle.Precio = request.Precio;
        detalle.ModifiedBy = request.Usuario;
        detalle.ModifiedAt = DateTime.Now;

        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    private static async Task<IResult> EliminarAsync(int id, AppDbContext db)
    {
        var detalle = await db.OrdenDetalles.SingleOrDefaultAsync(x => x.IdOrdenDetalles == id);

        if (detalle is null)
            return Results.NotFound();

        db.OrdenDetalles.Remove(detalle);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }
}

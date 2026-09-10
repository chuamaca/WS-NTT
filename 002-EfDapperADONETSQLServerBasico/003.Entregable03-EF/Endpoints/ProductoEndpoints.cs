using _003.Entregable03_EF.Data;
using _003.Entregable03_EF.Domain;
using _003.Entregable03_EF.Dtos;
using Microsoft.EntityFrameworkCore;

namespace _003.Entregable03_EF.Endpoints;

public static class ProductoEndpoints
{
    public static RouteGroupBuilder MapProductoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/productos").WithTags("Productos");

        group.MapGet("/", ListarAsync);
        group.MapGet("/{id:int}", ObtenerAsync);
        group.MapPost("/", InsertarAsync);
        group.MapPut("/{id:int}", ActualizarAsync);
        group.MapDelete("/{id:int}", EliminarAsync);

        return group;
    }

    private static async Task<IResult> ListarAsync(AppDbContext db)
    {
        var result = await db.Productos
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Nombre)
            .Select(x => new ProductoDto(
                x.IdProducto, x.IdCategoria, x.Categoria.Nombre, x.Nombre, x.Precio, x.Stock,
                x.State, x.CreatedAt, x.CreatedBy, x.ModifiedAt, x.ModifiedBy))
            .ToListAsync();

        return Results.Ok(result);
    }

    private static async Task<IResult> ObtenerAsync(int id, AppDbContext db)
    {
        var result = await db.Productos
            .AsNoTracking()
            .Where(x => x.IdProducto == id && !x.IsDeleted)
            .Select(x => new ProductoDto(
                x.IdProducto, x.IdCategoria, x.Categoria.Nombre, x.Nombre, x.Precio, x.Stock,
                x.State, x.CreatedAt, x.CreatedBy, x.ModifiedAt, x.ModifiedBy))
            .SingleOrDefaultAsync();

        return result is null ? Results.NotFound() : Results.Ok(result);
    }

    private static async Task<IResult> InsertarAsync(ProductoRequest request, AppDbContext db)
    {
        var producto = new Producto
        {
            IdCategoria = request.IdCategoria,
            Nombre = request.Nombre.Trim(),
            Precio = request.Precio,
            Stock = request.Stock,
            CreatedBy = request.Usuario,
            CreatedAt = DateTime.Now
        };

        db.Productos.Add(producto);
        await db.SaveChangesAsync();

        return Results.Created($"/api/productos/{producto.IdProducto}", new { producto.IdProducto });
    }

    private static async Task<IResult> ActualizarAsync(int id, ProductoRequest request, AppDbContext db)
    {
        var producto = await db.Productos.SingleOrDefaultAsync(x => x.IdProducto == id && !x.IsDeleted);

        if (producto is null)
            return Results.NotFound();

        producto.IdCategoria = request.IdCategoria;
        producto.Nombre = request.Nombre.Trim();
        producto.Precio = request.Precio;
        producto.Stock = request.Stock;
        producto.ModifiedBy = request.Usuario;
        producto.ModifiedAt = DateTime.Now;

        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    private static async Task<IResult> EliminarAsync(int id, AppDbContext db)
    {
        var producto = await db.Productos.SingleOrDefaultAsync(x => x.IdProducto == id);

        if (producto is null)
            return Results.NotFound();

        db.Productos.Remove(producto);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }
}

using _003.Entregable03_EF.Data;
using _003.Entregable03_EF.Domain;
using _003.Entregable03_EF.Dtos;
using Microsoft.EntityFrameworkCore;

namespace _003.Entregable03_EF.Endpoints;

public static class CategoriaEndpoints
{
    public static RouteGroupBuilder MapCategoriaEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/categorias").WithTags("Categorias");

        group.MapGet("/", ListarAsync);
        group.MapGet("/{id:int}", ObtenerAsync);
        group.MapPost("/", InsertarAsync);
        group.MapPut("/{id:int}", ActualizarAsync);
        group.MapDelete("/{id:int}", EliminarAsync);

        return group;
    }

    private static async Task<IResult> ListarAsync(AppDbContext db)
    {
        var result = await db.Categorias
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Nombre)
            .Select(x => new CategoriaDto(
                x.IdCategoria, x.Nombre, x.State, x.CreatedAt, x.CreatedBy, x.ModifiedAt, x.ModifiedBy))
            .ToListAsync();

        return Results.Ok(result);
    }

    private static async Task<IResult> ObtenerAsync(int id, AppDbContext db)
    {
        var result = await db.Categorias
            .AsNoTracking()
            .Where(x => x.IdCategoria == id && !x.IsDeleted)
            .Select(x => new CategoriaDto(
                x.IdCategoria, x.Nombre, x.State, x.CreatedAt, x.CreatedBy, x.ModifiedAt, x.ModifiedBy))
            .SingleOrDefaultAsync();

        return result is null ? Results.NotFound() : Results.Ok(result);
    }

    private static async Task<IResult> InsertarAsync(CategoriaRequest request, AppDbContext db)
    {
        var categoria = new Categoria
        {
            Nombre = request.Nombre.Trim(),
            CreatedBy = request.Usuario,
            CreatedAt = DateTime.Now
        };

        db.Categorias.Add(categoria);
        await db.SaveChangesAsync();

        return Results.Created($"/api/categorias/{categoria.IdCategoria}", new { categoria.IdCategoria });
    }

    private static async Task<IResult> ActualizarAsync(int id, CategoriaRequest request, AppDbContext db)
    {
        var categoria = await db.Categorias.SingleOrDefaultAsync(x => x.IdCategoria == id && !x.IsDeleted);

        if (categoria is null)
            return Results.NotFound();

        categoria.Nombre = request.Nombre.Trim();
        categoria.ModifiedBy = request.Usuario;
        categoria.ModifiedAt = DateTime.Now;

        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    private static async Task<IResult> EliminarAsync(int id, AppDbContext db)
    {
        var categoria = await db.Categorias.SingleOrDefaultAsync(x => x.IdCategoria == id);

        if (categoria is null)
            return Results.NotFound();

        db.Categorias.Remove(categoria);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }
}

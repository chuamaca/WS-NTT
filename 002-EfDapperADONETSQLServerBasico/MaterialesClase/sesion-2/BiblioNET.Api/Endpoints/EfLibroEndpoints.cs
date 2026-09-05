using BiblioNET.Api.Data;
using BiblioNET.Api.Domain;
using BiblioNET.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BiblioNET.Api.Endpoints;

public static class EfLibroEndpoints
{
    public static RouteGroupBuilder MapEfLibroEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/ef/libros")
            .WithTags("Entity Framework Core");

        group.MapGet("/", ListarAsync);
        group.MapGet("/{id:int}", ObtenerAsync);
        group.MapPost("/", InsertarAsync);
        group.MapPut("/{id:int}", ActualizarAsync);
        group.MapDelete("/{id:int}", EliminarAsync);

        return group;
    }

    private static async Task<IResult> ListarAsync(BiblioNetDbContext db)
    {
        var result = await db.Libros
            .AsNoTracking()
            .Where(x => x.Activo)
            .OrderBy(x => x.Titulo)
            .Select(x => new LibroDto(
                x.LibroId,
                x.GeneroId,
                x.Genero.Nombre,
                x.Titulo,
                x.Autor,
                x.ISBN,
                x.Stock))
            .ToListAsync();

        return Results.Ok(result);
    }

    private static async Task<IResult> ObtenerAsync(
        int id,
        BiblioNetDbContext db)
    {
        var result = await db.Libros
            .AsNoTracking()
            .Where(x => x.LibroId == id)
            .Select(x => new LibroDto(
                x.LibroId,
                x.GeneroId,
                x.Genero.Nombre,
                x.Titulo,
                x.Autor,
                x.ISBN,
                x.Stock))
            .SingleOrDefaultAsync();

        return result is null
            ? Results.NotFound()
            : Results.Ok(result);
    }

    private static async Task<IResult> InsertarAsync(
        LibroRequest request,
        BiblioNetDbContext db)
    {
        var libro = new Libro
        {
            GeneroId = request.GeneroId,
            Titulo = request.Titulo.Trim(),
            Autor = request.Autor.Trim(),
            ISBN = request.ISBN.Trim(),
            Stock = request.Stock,
            Activo = true,
            FechaRegistro = DateTime.Now
        };

        db.Libros.Add(libro);
        await db.SaveChangesAsync();

        return Results.Created(
            $"/api/ef/libros/{libro.LibroId}",
            new { libro.LibroId });
    }

    private static async Task<IResult> ActualizarAsync(
        int id,
        LibroRequest request,
        BiblioNetDbContext db)
    {
        var libro = await db.Libros.FindAsync(id);

        if (libro is null)
            return Results.NotFound();

        libro.GeneroId = request.GeneroId;
        libro.Titulo = request.Titulo.Trim();
        libro.Autor = request.Autor.Trim();
        libro.ISBN = request.ISBN.Trim();
        libro.Stock = request.Stock;

        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    private static async Task<IResult> EliminarAsync(
        int id,
        BiblioNetDbContext db)
    {
        var libro = await db.Libros.FindAsync(id);

        if (libro is null)
            return Results.NotFound();

        db.Libros.Remove(libro);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }
}

using _003.Entregable03_EF.Data;
using _003.Entregable03_EF.Domain;
using _003.Entregable03_EF.Dtos;
using Microsoft.EntityFrameworkCore;

namespace _003.Entregable03_EF.Endpoints;

public static class ClienteEndpoints
{
    public static RouteGroupBuilder MapClienteEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/clientes").WithTags("Clientes");

        group.MapGet("/", ListarAsync);
        group.MapGet("/{id:int}", ObtenerAsync);
        group.MapPost("/", InsertarAsync);
        group.MapPut("/{id:int}", ActualizarAsync);
        group.MapDelete("/{id:int}", EliminarAsync);

        return group;
    }

    private static async Task<IResult> ListarAsync(AppDbContext db)
    {
        var result = await db.Clientes
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Nombre).ThenBy(x => x.Apellido)
            .Select(x => new ClienteDto(
                x.IdCliente, x.Nombre, x.Apellido, x.Email, x.Telefono, x.Direccion, x.Documento,
                x.State, x.CreatedAt, x.CreatedBy, x.ModifiedAt, x.ModifiedBy))
            .ToListAsync();

        return Results.Ok(result);
    }

    private static async Task<IResult> ObtenerAsync(int id, AppDbContext db)
    {
        var result = await db.Clientes
            .AsNoTracking()
            .Where(x => x.IdCliente == id && !x.IsDeleted)
            .Select(x => new ClienteDto(
                x.IdCliente, x.Nombre, x.Apellido, x.Email, x.Telefono, x.Direccion, x.Documento,
                x.State, x.CreatedAt, x.CreatedBy, x.ModifiedAt, x.ModifiedBy))
            .SingleOrDefaultAsync();

        return result is null ? Results.NotFound() : Results.Ok(result);
    }

    private static async Task<IResult> InsertarAsync(ClienteRequest request, AppDbContext db)
    {
        var cliente = new Cliente
        {
            Nombre = request.Nombre.Trim(),
            Apellido = request.Apellido.Trim(),
            Email = request.Email.Trim(),
            Telefono = request.Telefono.Trim(),
            Direccion = request.Direccion.Trim(),
            Documento = request.Documento.Trim(),
            CreatedBy = request.Usuario,
            CreatedAt = DateTime.Now
        };

        db.Clientes.Add(cliente);
        await db.SaveChangesAsync();

        return Results.Created($"/api/clientes/{cliente.IdCliente}", new { cliente.IdCliente });
    }

    private static async Task<IResult> ActualizarAsync(int id, ClienteRequest request, AppDbContext db)
    {
        var cliente = await db.Clientes.SingleOrDefaultAsync(x => x.IdCliente == id && !x.IsDeleted);

        if (cliente is null)
            return Results.NotFound();

        cliente.Nombre = request.Nombre.Trim();
        cliente.Apellido = request.Apellido.Trim();
        cliente.Email = request.Email.Trim();
        cliente.Telefono = request.Telefono.Trim();
        cliente.Direccion = request.Direccion.Trim();
        cliente.Documento = request.Documento.Trim();
        cliente.ModifiedBy = request.Usuario;
        cliente.ModifiedAt = DateTime.Now;

        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    private static async Task<IResult> EliminarAsync(int id, AppDbContext db)
    {
        var cliente = await db.Clientes.SingleOrDefaultAsync(x => x.IdCliente == id);

        if (cliente is null)
            return Results.NotFound();

        db.Clientes.Remove(cliente);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }
}

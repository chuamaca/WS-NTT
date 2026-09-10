using _003.Entregable03_EF.Data;
using _003.Entregable03_EF.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'DefaultConnection'.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    Aplicacion = "Entregable03 - Entity Framework Core",
    Tecnologia = "Entity Framework Core + SQL Server"
}));

app.MapCategoriaEndpoints();
app.MapProductoEndpoints();
app.MapClienteEndpoints();
app.MapOrdenEndpoints();
app.MapOrdenDetalleEndpoints();

app.Run();

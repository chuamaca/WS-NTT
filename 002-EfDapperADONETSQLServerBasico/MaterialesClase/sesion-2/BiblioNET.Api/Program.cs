using BiblioNET.Api.Data;
using BiblioNET.Api.Data.Dapper;
using BiblioNET.Api.Endpoints;
using BiblioNET.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("BiblioNET") 
    ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'BiblioNET'.");

builder.Services.AddDbContext<BiblioNetDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();

builder.Services.AddScoped<ILibroDapperRepository, LibroDapperRepository>();
builder.Services.AddScoped<IBenchmarkDapperRepository, BenchmarkDapperRepository>();
builder.Services.AddScoped<PrestamoDapperRepository>();

builder.Services.AddScoped<BenchmarkEfService>();
builder.Services.AddSingleton<BenchmarkRunner>();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    Aplicacion = "BiblioNET API",
    Sesion = 2,
    Tecnologias = new[] { "Entity Framework Core", "Dapper", "SQL Server" }
}));

app.MapEfLibroEndpoints();
app.MapDapperLibroEndpoints();

app.MapEfPrestamoEndpoints();
app.MapDapperPrestamoEndpoints();

app.MapBenchmarkEndpoints();

app.Run();
using _004.Entregable04_Dapper.Cross;
using _004.Entregable04_Dapper.Data;
using _004.Entregable04_Dapper.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
builder.Services.AddScoped<IClienteDapperRepository, ClienteDapperRepository>();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    Aplicacion = "Entregable04 - Dapper",
    Tecnologia = "Dapper + SQL Server"
}));

app.MapClienteEndpoints();

app.Run();

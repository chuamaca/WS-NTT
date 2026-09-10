using _002.Entregable02_AdoNet.Cross;
using _002.Entregable02_AdoNet.Data;
using _002.Entregable02_AdoNet.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

builder.Services.AddScoped<ICategoriaDataAccess, CategoriaDataAccess>();
builder.Services.AddScoped<IProductoDataAccess, ProductoDataAccess>();
builder.Services.AddScoped<IClienteDataAccess, ClienteDataAccess>();
builder.Services.AddScoped<IOrdenDataAccess, OrdenDataAccess>();
builder.Services.AddScoped<IOrdenDetalleDataAccess, OrdenDetalleDataAccess>();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    Aplicacion = "Entregable02 - ADO.NET",
    Tecnologia = "ADO.NET (Microsoft.Data.SqlClient)"
}));

app.MapCategoriaEndpoints();
app.MapProductoEndpoints();
app.MapClienteEndpoints();
app.MapOrdenEndpoints();
app.MapOrdenDetalleEndpoints();

app.Run();

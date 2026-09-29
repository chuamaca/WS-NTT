using DemoApi.Data;
using DemoApi.Dtos;
using DemoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Extensions;

public static class ProductEndpointExtensions
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/products", async (AppDbContext db) =>
        {
            var products = await db.Products.Include(p => p.Category).OrderBy(p => p.Id).ToListAsync();
            return Results.Ok(new ApiResponse<List<Product>>
            {
                IsSuccess = true,
                Message = "Productos obtenidos correctamente.",
                Data = products
            });
        })
        .WithName("GetProducts")
        .WithTags("Product")
        .RequireAuthorization()
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<ApiResponse<List<Product>>>(StatusCodes.Status200OK);

        app.MapGet("/api/products/{id:int}", async (int id, AppDbContext db) =>
        {
            var product = await db.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (product is null)
            {
                return Results.NotFound(new ApiResponse<Product>
                {
                    IsSuccess = false,
                    Message = "Producto no encontrado.",
                    Data = null
                });
            }

            return Results.Ok(new ApiResponse<Product>
            {
                IsSuccess = true,
                Message = "Producto obtenido correctamente.",
                Data = product
            });
        })
        .WithName("GetProductById")
        .WithTags("Product")
        .RequireAuthorization()
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<ApiResponse<Product>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<Product>>(StatusCodes.Status404NotFound);

        app.MapPost("/api/products", async (CreateProductRequestDto request, AppDbContext db) =>
        {
            var errors = request.GetValidationErrors();
            if (errors.Count > 0)
            {
                return Results.BadRequest(new ApiResponse<Product>
                {
                    IsSuccess = false,
                    Message = "La solicitud contiene datos inválidos.",
                    Data = null,
                    Errors = errors
                });
            }

            var exists = await db.Products.AnyAsync(p => p.Sku == request.Sku);
            if (exists)
                return Results.Conflict(new ApiResponse<Product>
                {
                    IsSuccess = false,
                    Message = "Sku already exists.",
                    Data = null,
                    Errors = new List<string> { "The provided Sku already exists." }
                });

            var categoryExists = await db.Categories.AnyAsync(c => c.Id == request.CategoryId);
            if (!categoryExists)
                return Results.BadRequest(new ApiResponse<Product>
                {
                    IsSuccess = false,
                    Message = "La categoría indicada no existe.",
                    Data = null,
                    Errors = new List<string> { "The provided CategoryId does not exist." }
                });

            var product = new Product
            {
                Name = request.Name,
                Sku = request.Sku,
                Price = request.Price,
                CategoryId = request.CategoryId,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            db.Products.Add(product);
            await db.SaveChangesAsync();

            return Results.Created($"/api/products/{product.Id}", new ApiResponse<Product>
            {
                IsSuccess = true,
                Message = "Producto creado correctamente.",
                Data = product,
                Errors = new List<string>()
            });
        })
        .WithName("CreateProduct")
        .WithTags("Product")
        .RequireAuthorization()
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<ApiResponse<Product>>(StatusCodes.Status201Created)
        .Produces<ApiResponse<Product>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<Product>>(StatusCodes.Status409Conflict);

        app.MapPut("/api/products/{id:int}", async (int id, UpdateProductRequestDto request, AppDbContext db) =>
        {
            var errors = request.GetValidationErrors();
            if (errors.Count > 0)
            {
                return Results.BadRequest(new ApiResponse<Product>
                {
                    IsSuccess = false,
                    Message = "La solicitud contiene datos inválidos.",
                    Data = null,
                    Errors = errors
                });
            }

            var product = await db.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (product is null)
                return Results.NotFound(new ApiResponse<Product>
                {
                    IsSuccess = false,
                    Message = "Producto no encontrado.",
                    Data = null,
                    Errors = new List<string> { "Producto no encontrado." }
                });

            var duplicateSku = await db.Products.AnyAsync(p => p.Sku == request.Sku && p.Id != id);
            if (duplicateSku)
                return Results.Conflict(new ApiResponse<Product>
                {
                    IsSuccess = false,
                    Message = "Sku already exists.",
                    Data = null,
                    Errors = new List<string> { "The provided Sku already exists." }
                });

            var categoryExists = await db.Categories.AnyAsync(c => c.Id == request.CategoryId);
            if (!categoryExists)
                return Results.BadRequest(new ApiResponse<Product>
                {
                    IsSuccess = false,
                    Message = "La categoría indicada no existe.",
                    Data = null,
                    Errors = new List<string> { "The provided CategoryId does not exist." }
                });

            product.Name = request.Name;
            product.Sku = request.Sku;
            product.Price = request.Price;
            product.CategoryId = request.CategoryId;
            product.IsActive = request.IsActive;

            await db.SaveChangesAsync();
            return Results.Ok(new ApiResponse<Product>
            {
                IsSuccess = true,
                Message = "Producto actualizado correctamente.",
                Data = product,
                Errors = new List<string>()
            });
        })
        .WithName("UpdateProduct")
        .WithTags("Product")
        .RequireAuthorization()
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<ApiResponse<Product>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<Product>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<Product>>(StatusCodes.Status404NotFound)
        .Produces<ApiResponse<Product>>(StatusCodes.Status409Conflict);

        app.MapDelete("/api/products/{id:int}", async (int id, AppDbContext db) =>
        {
            var product = await db.Products.FindAsync(id);
            if (product is null)
                return Results.NotFound(new ApiResponse<Product>
                {
                    IsSuccess = false,
                    Message = "Producto no encontrado.",
                    Data = null,
                    Errors = new List<string> { "Producto no encontrado." }
                });

            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithName("DeleteProduct")
        .WithTags("Product")
        .RequireAuthorization()
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ApiResponse<Product>>(StatusCodes.Status404NotFound);

        return app;
    }
}
using DemoApi.Data;
using DemoApi.Dtos;
using DemoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Extensions;

public static class CategoryEndpointExtensions
{
    public static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/categories", async (AppDbContext db) =>
        {
            var categories = await db.Categories.OrderBy(c => c.Id).ToListAsync();
            return Results.Ok(new ApiResponse<List<Category>>
            {
                IsSuccess = true,
                Message = "Categorías obtenidas correctamente.",
                Data = categories
            });
        })
        .WithName("GetCategories")
        .WithTags("Category")
        .RequireAuthorization()
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<ApiResponse<List<Category>>>(StatusCodes.Status200OK);

        app.MapGet("/api/categories/{id:int}", async (int id, AppDbContext db) =>
        {
            var category = await db.Categories.FindAsync(id);
            if (category is null)
                return Results.NotFound(new ApiResponse<Category>
                {
                    IsSuccess = false,
                    Message = "Categoría no encontrada.",
                    Errors = new List<string> { "Category not found." }
                });

            return Results.Ok(new ApiResponse<Category>
            {
                IsSuccess = true,
                Message = "Categoría obtenida correctamente.",
                Data = category
            });
        })
        .WithName("GetCategoryById")
        .WithTags("Category")
        .RequireAuthorization()
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<ApiResponse<Category>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<Category>>(StatusCodes.Status404NotFound);

        app.MapPost("/api/categories", async (CreateCategoryRequestDto request, AppDbContext db) =>
        {
            var errors = request.GetValidationErrors();
            if (errors.Count > 0)
                return Results.BadRequest(new ApiResponse<Category>
                {
                    IsSuccess = false,
                    Message = "La solicitud contiene datos inválidos.",
                    Errors = errors
                });

            var exists = await db.Categories.AnyAsync(c => c.Name == request.Name);
            if (exists)
                return Results.Conflict(new ApiResponse<Category>
                {
                    IsSuccess = false,
                    Message = "Category name already exists.",
                    Errors = new List<string> { "The provided category name already exists." }
                });

            var category = new Category
            {
                Name = request.Name,
                Description = request.Description
            };

            db.Categories.Add(category);
            await db.SaveChangesAsync();

            return Results.Created($"/api/categories/{category.Id}", new ApiResponse<Category>
            {
                IsSuccess = true,
                Message = "Categoría creada correctamente.",
                Data = category
            });
        })
        .WithName("CreateCategory")
        .WithTags("Category")
        .RequireAuthorization()
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<ApiResponse<Category>>(StatusCodes.Status201Created)
        .Produces<ApiResponse<Category>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<Category>>(StatusCodes.Status409Conflict);

        app.MapPut("/api/categories/{id:int}", async (int id, UpdateCategoryRequestDto request, AppDbContext db) =>
        {
            var errors = request.GetValidationErrors();
            if (errors.Count > 0)
                return Results.BadRequest(new ApiResponse<Category>
                {
                    IsSuccess = false,
                    Message = "La solicitud contiene datos inválidos.",
                    Errors = errors
                });

            var category = await db.Categories.FindAsync(id);
            if (category is null)
                return Results.NotFound(new ApiResponse<Category>
                {
                    IsSuccess = false,
                    Message = "Categoría no encontrada.",
                    Errors = new List<string> { "Category not found." }
                });

            var duplicateName = await db.Categories.AnyAsync(c => c.Name == request.Name && c.Id != id);
            if (duplicateName)
                return Results.Conflict(new ApiResponse<Category>
                {
                    IsSuccess = false,
                    Message = "Category name already exists.",
                    Errors = new List<string> { "The provided category name already exists." }
                });

            category.Name = request.Name;
            category.Description = request.Description;
            await db.SaveChangesAsync();

            return Results.Ok(new ApiResponse<Category>
            {
                IsSuccess = true,
                Message = "Categoría actualizada correctamente.",
                Data = category
            });
        })
        .WithName("UpdateCategory")
        .WithTags("Category")
        .RequireAuthorization()
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<ApiResponse<Category>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<Category>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<Category>>(StatusCodes.Status404NotFound)
        .Produces<ApiResponse<Category>>(StatusCodes.Status409Conflict);

        app.MapDelete("/api/categories/{id:int}", async (int id, AppDbContext db) =>
        {
            var category = await db.Categories.FindAsync(id);
            if (category is null)
                return Results.NotFound(new ApiResponse<Category>
                {
                    IsSuccess = false,
                    Message = "Categoría no encontrada.",
                    Errors = new List<string> { "Category not found." }
                });

            var hasProducts = await db.Products.AnyAsync(p => p.CategoryId == id);
            if (hasProducts)
                return Results.Conflict(new ApiResponse<Category>
                {
                    IsSuccess = false,
                    Message = "No se puede eliminar una categoría con productos asociados.",
                    Errors = new List<string> { "The category has associated products." }
                });

            db.Categories.Remove(category);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithName("DeleteCategory")
        .WithTags("Category")
        .RequireAuthorization()
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ApiResponse<Category>>(StatusCodes.Status404NotFound)
        .Produces<ApiResponse<Category>>(StatusCodes.Status409Conflict);

        return app;
    }
}
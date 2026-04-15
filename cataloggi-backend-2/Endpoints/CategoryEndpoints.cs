using cataloggi_backend_2.DTOs.Category;
using cataloggi_backend_2.Services.Interfaces;

namespace cataloggi_backend_2.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/categories");

        group.MapGet("/", async (ICategoryService categoryService) =>
        {
            var categories = await categoryService.GetCategories();
            return Results.Ok(categories);
        });

        group.MapGet("/{id:guid}", async (Guid id, ICategoryService categoryService) =>
        {
            var category = await categoryService.GetCategory(id);

            return category is null
                ? Results.NotFound(new { message = "Category not found" })
                : Results.Ok(category);
        });

        group.MapPost("/", async (CreateCategoryDto dto, ICategoryService categoryService) =>
        {
            var createdCategory = await categoryService.CreateCategory(dto);
            return Results.Created($"/api/categories/{createdCategory.Id}", createdCategory);
        });

        group.MapPut("/{id:guid}", async (Guid id, UpdateCategoryDto dto, ICategoryService categoryService) =>
        {
            var updated = await categoryService.UpdateCategory(id, dto);

            return updated is null
                ? Results.NotFound(new { message = "Category not found" })
                : Results.Ok(updated);
        });

        group.MapDelete("/{id:guid}", async (Guid id, ICategoryService categoryService) =>
        {
            var deleted = await categoryService.DeleteCategory(id);

            return deleted
                ? Results.NoContent()
                : Results.NotFound(new { message = "Category not found" });
        });
    }
}
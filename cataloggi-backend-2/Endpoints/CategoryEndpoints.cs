using System.ComponentModel.DataAnnotations;
using cataloggi_backend_2.DTOs.Category;
using cataloggi_backend_2.Exceptions;
using cataloggi_backend_2.RateLimiting;
using cataloggi_backend_2.Services.Interfaces;

namespace cataloggi_backend_2.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/categories")
            .RequireAuthorization()
            .WithTags("Categories");

        async Task<IResult> GetCategories(ICategoryService categoryService)
        {
            var categories = await categoryService.GetCategories();
            return Results.Ok(categories);
        }

        group.MapGet("/", GetCategories)
        .RequireRateLimiting(RateLimitPolicies.Read);

        group.MapGet("/{id:guid}", async (Guid id, ICategoryService categoryService) =>
        {
            try
            {
                var category = await categoryService.GetCategory(id);
                return Results.Ok(category);
            }
            catch (NotFoundException ex)
            {
                return Results.NotFound(new { message = ex.Message });
            }
        })
        .RequireRateLimiting(RateLimitPolicies.Read);

        group.MapPost("/", async (CreateCategoryDto? dto, ICategoryService categoryService) =>
        {
            if (dto is null)
                return Results.BadRequest(new { errors = new[] { "Request body is required" } });
            
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(dto);

            if (!Validator.TryValidateObject(dto, context, validationResults, true))
            {
                var errors = validationResults.Select(x => x.ErrorMessage);
                return Results.BadRequest(new { errors });
            }
            
            var createdCategory = await categoryService.CreateCategory(dto);
            return Results.Created($"/api/categories/{createdCategory.Id}", createdCategory);
        })
        .RequireRateLimiting(RateLimitPolicies.Write);

        group.MapPut("/{id:guid}", async (Guid id, UpdateCategoryDto? dto, ICategoryService categoryService) =>
        {
            if (dto is null)
                return Results.BadRequest(new { errors = new[] { "Request body is required" } });
            
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(dto);

            if (!Validator.TryValidateObject(dto, context, validationResults, true))
            {
                var errors = validationResults.Select(x => x.ErrorMessage);
                return Results.BadRequest(new { errors });
            }
            
            try
            {
                var updated = await categoryService.UpdateCategory(id, dto);
                return Results.Ok(updated);
            }
            catch (NotFoundException ex)
            {
                return Results.NotFound(new { message = ex.Message });
            }
            catch (ConflictException ex)
            {
                return Results.Conflict(new { message = ex.Message });
            }
        })
        .RequireRateLimiting(RateLimitPolicies.Write);

        group.MapDelete("/{id:guid}", async (Guid id, ICategoryService categoryService) =>
        {
            try
            {
                await categoryService.DeleteCategory(id);
                return Results.NoContent();
            }
            catch (NotFoundException ex)
            {
                return Results.NotFound(new { message = ex.Message });
            }
        })
        .RequireRateLimiting(RateLimitPolicies.Write);
    }
}

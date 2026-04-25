using System.ComponentModel.DataAnnotations;
using cataloggi_backend_2.DTOs;
using cataloggi_backend_2.Exceptions;
using cataloggi_backend_2.RateLimiting;
using cataloggi_backend_2.Services.Interfaces;

namespace cataloggi_backend_2.Endpoints;

public static class ItemEndpoints
{
    public static void MapItemEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/items");

        group.MapGet("/", async (IItemService itemService) =>
        {
            var items = await itemService.GetItems();
            return Results.Ok(items);
        })
        .RequireRateLimiting(RateLimitPolicies.Read);

        group.MapGet("/{id:guid}", async (Guid id, IItemService itemService) =>
        {
            try
            {
                var item = await itemService.GetItem(id);
                return Results.Ok(item);
            }
            catch (NotFoundException ex)
            {
                return Results.NotFound(new { message = ex.Message });
            }
        })
        .RequireRateLimiting(RateLimitPolicies.Read);

        group.MapGet("/summaries", async (IItemService itemService) =>
        {
            var itemSummaries = await itemService.GetItemSummaries();

            return Results.Ok(itemSummaries);
        })
        .RequireRateLimiting(RateLimitPolicies.Read);

        group.MapPost("/", async (CreateItemDto? dto, IItemService itemService) =>
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
            
            var createdItem = await itemService.CreateItem((dto));
            
            return Results.Created($"/api/items/{createdItem.Id}", createdItem);
        })
        .RequireRateLimiting(RateLimitPolicies.Write);

        group.MapPut("/{id:guid}", async (Guid id, UpdateItemDto? dto, IItemService itemService) =>
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
                var updated = await itemService.UpdateItem(id, dto);
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

        group.MapDelete("/{id:guid}", async (Guid id, IItemService itemService) =>
        {
            try
            {
                await itemService.DeleteItem(id);
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

using cataloggi_backend_2.DTOs;
using cataloggi_backend_2.DTOs.Sync;
using cataloggi_backend_2.RateLimiting;
using cataloggi_backend_2.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace cataloggi_backend_2.Controllers;

[ApiController]
[Authorize]
[Route("api")]
[Produces("application/json")]
[Tags("Sync")]
public class SyncController(ISyncService syncService) : ControllerBase
{
    [HttpGet("sync-categories")]
    [AllowAnonymous]
    [EnableRateLimiting(RateLimitPolicies.Read)]
    [ProducesResponseType(typeof(CategorySyncResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<CategorySyncResponseDto>> SyncCategories(
        [FromQuery] DateTime? since = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 500)
    {
        var result = await syncService.SyncCategories(since, page, pageSize);
        return Ok(result);
    }

    [HttpGet("sync-items")]
    [AllowAnonymous]
    [EnableRateLimiting(RateLimitPolicies.Read)]
    [ProducesResponseType(typeof(ItemSyncResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<ItemSyncResponseDto>> SyncItems(
        [FromQuery] DateTime? since = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 500)
    {
        var result = await syncService.SyncItems(since, page, pageSize);
        return Ok(result);
    }

    [HttpPost("items/batch")]
    [AllowAnonymous]
    [EnableRateLimiting(RateLimitPolicies.Read)]
    [ProducesResponseType(typeof(List<ItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<List<ItemDto>>> GetItemsBatch(
        [FromBody] BatchItemRequestDto request)
    {
        if (request.Ids is null || request.Ids.Count == 0)
            return ValidationProblem("At least one item ID is required.");

        if (request.Ids.Count > 500)
            return ValidationProblem("Maximum 500 items per batch request.");

        var items = await syncService.GetItemsByIds(request.Ids);
        return Ok(items);
    }
}

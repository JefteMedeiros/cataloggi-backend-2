using cataloggi_backend_2.DTOs;
using cataloggi_backend_2.Exceptions;
using cataloggi_backend_2.RateLimiting;
using cataloggi_backend_2.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace cataloggi_backend_2.Controllers;

[ApiController]
[Authorize]
[Route("api/items")]
[Produces("application/json")]
[Tags("Items")]
public class ItemsController(IItemService itemService) : ControllerBase
{
    [HttpGet]
    [EnableRateLimiting(RateLimitPolicies.Read)]
    [ProducesResponseType(typeof(PaginatedResponseDto<ItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<PaginatedResponseDto<ItemDto>>> 
        GetItems([FromQuery]int page = 1, [FromQuery]int pageSize = 10)
    {
        var items = await itemService.GetItems(page, pageSize);
        return Ok(items);
    }

    [HttpGet("summaries")]
    [EnableRateLimiting(RateLimitPolicies.Read)]
    [ProducesResponseType(typeof(PaginatedResponseDto<ItemSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<PaginatedResponseDto<ItemSummaryDto>>> 
        GetItemSummaries([FromQuery]int page = 1, [FromQuery]int pageSize = 10)
    {
        var itemSummaries = await itemService
            .GetItemSummaries(page, pageSize);
        return Ok(itemSummaries);
    }

    [HttpGet("{id:guid}")]
    [EnableRateLimiting(RateLimitPolicies.Read)]
    [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<ItemDto>> GetItem(Guid id)
    {
        try
        {
            var item = await itemService.GetItem(id);
            return Ok(item);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErrorResponseDto { Message = ex.Message });
        }
    }

    [HttpPost]
    [EnableRateLimiting(RateLimitPolicies.Write)]
    [ProducesResponseType(typeof(ItemDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<ItemDto>> CreateItem(CreateItemRequestDto dto)
    {
        var itemDto = new CreateItemDto
        {
            CategoryId = Guid.Parse(dto.CategoryId!),
            Name = dto.Name,
            Content = dto.Content
        };

        try
        {
            var createdItem = await itemService.CreateItem(itemDto);
            return CreatedAtAction(nameof(GetItem), new { id = createdItem.Id }, createdItem);
        }
        catch (BadRequestException ex)
        {
            ModelState.AddModelError(nameof(dto.CategoryId), ex.Message);
            return ValidationProblem(ModelState);
        }
    }

    [HttpPut("{id:guid}")]
    [EnableRateLimiting(RateLimitPolicies.Write)]
    [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<ItemDto>> UpdateItem(Guid id, UpdateItemRequestDto dto)
    {
        var itemDto = new UpdateItemDto
        {
            CategoryId = Guid.Parse(dto.CategoryId!),
            Name = dto.Name,
            Content = dto.Content
        };

        try
        {
            var updated = await itemService.UpdateItem(id, itemDto);
            return Ok(updated);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErrorResponseDto { Message = ex.Message });
        }
        catch (BadRequestException ex)
        {
            ModelState.AddModelError(nameof(dto.CategoryId), ex.Message);
            return ValidationProblem(ModelState);
        }
        catch (ConflictException ex)
        {
            return Conflict(new ErrorResponseDto { Message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [EnableRateLimiting(RateLimitPolicies.Write)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> DeleteItem(Guid id)
    {
        try
        {
            await itemService.DeleteItem(id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErrorResponseDto { Message = ex.Message });
        }
    }
}

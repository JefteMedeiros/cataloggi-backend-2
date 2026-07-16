using cataloggi_backend_2.DTOs;
using cataloggi_backend_2.DTOs.Category;
using cataloggi_backend_2.Exceptions;
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
[Tags("Categories")]
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet("categories")]
    [AllowAnonymous]
    [EnableRateLimiting(RateLimitPolicies.Read)]
    [ProducesResponseType(typeof(PaginatedResponseDto<CategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<PaginatedResponseDto<CategoryDto>>> 
        GetCategories([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = "")
    {
        var categories = await 
            categoryService.GetCategories(page, pageSize, search);
        return Ok(categories);
    }

    [HttpGet("category/{id:guid}")]
    [AllowAnonymous]
    [EnableRateLimiting(RateLimitPolicies.Read)]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<CategoryDto>> GetCategory(Guid id)
    {
        try
        {
            var category = await categoryService.GetCategory(id);
            return Ok(category);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErrorResponseDto { Message = ex.Message });
        }
    }

    [HttpPost("create-category")]
    [EnableRateLimiting(RateLimitPolicies.Write)]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryDto dto)
    {
        var createdCategory = await categoryService.CreateCategory(dto);
        return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.Id }, createdCategory);
    }

    [HttpPut("update-category/{id:guid}")]
    [EnableRateLimiting(RateLimitPolicies.Write)]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<CategoryDto>> UpdateCategory(Guid id, UpdateCategoryDto dto)
    {
        try
        {
            var updated = await categoryService.UpdateCategory(id, dto);
            return Ok(updated);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErrorResponseDto { Message = ex.Message });
        }
        catch (ConflictException ex)
        {
            return Conflict(new ErrorResponseDto { Message = ex.Message });
        }
    }

    [HttpDelete("delete-category/{id:guid}")]
    [EnableRateLimiting(RateLimitPolicies.Write)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        try
        {
            await categoryService.DeleteCategory(id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ErrorResponseDto { Message = ex.Message });
        }
    }
}

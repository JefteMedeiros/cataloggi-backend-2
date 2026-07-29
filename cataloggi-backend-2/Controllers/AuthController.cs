using cataloggi_backend_2.DTOs.Auth;
using cataloggi_backend_2.RateLimiting;
using cataloggi_backend_2.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace cataloggi_backend_2.Controllers;

[ApiController]
[Route("api")]
[Produces("application/json")]
[Tags("Auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    [EnableRateLimiting(RateLimitPolicies.Login)]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status429TooManyRequests)]
    public ActionResult<LoginResponseDto> Login(LoginRequestDto dto)
    {
        var response = authService.Login(dto);

        return response is null
            ? Unauthorized()
            : Ok(response);
    }
}

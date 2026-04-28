using System.ComponentModel.DataAnnotations;
using cataloggi_backend_2.DTOs.Auth;
using cataloggi_backend_2.RateLimiting;
using cataloggi_backend_2.Services.Interfaces;

namespace cataloggi_backend_2.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Auth");

        group.MapPost("/login", (LoginRequestDto? dto, IAuthService authService) =>
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

            var response = authService.Login(dto);

            return response is null
                ? Results.Unauthorized()
                : Results.Ok(response);
        })
        .AllowAnonymous()
        .RequireRateLimiting(RateLimitPolicies.Write);
    }
}

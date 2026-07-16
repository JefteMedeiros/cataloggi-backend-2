using cataloggi_backend_2.AppDbContext;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace cataloggi_backend_2.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication ConfigureMiddlewarePipeline(
        this WebApplication app, WebApplicationBuilder builder)
    {
        app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
                var logger = context.RequestServices
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger("GlobalExceptionHandler");

                if (exception is not null)
                    logger.LogError(exception, "An unhandled exception occurred.");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsJsonAsync(new
                {
                    message = "An unexpected error occurred."
                });
            });
        });

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        if (app.Environment.IsDevelopment()
            || builder.Configuration.GetValue<bool>("Database:RunMigrationsOnStartup"))
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.Migrate();
        }

        app.UseCors("ConfiguredOrigins");

        if (!app.Environment.IsDevelopment())
            app.UseHttpsRedirection();

        app.UseSerilogRequestLogging();
        app.UseRateLimiter();
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}

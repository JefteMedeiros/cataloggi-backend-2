using cataloggi_backend_2.Extensions;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, _, loggerConfiguration) =>
{
    loggerConfiguration
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", builder.Environment.ApplicationName)
        .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
        .WriteTo.Console();
});

builder.Services
    .ConfigureDatabase(builder.Configuration)
    .ConfigureAuthentication(builder.Configuration)
    .ConfigureCors(builder.Configuration)
    .ConfigureRateLimiting()
    .ConfigureSwagger()
    .ConfigureRepositories()
    .AddControllers();

var app = builder.Build();

app.ConfigureMiddlewarePipeline(builder);

app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }))
    .AllowAnonymous()
    .WithTags("Health")
    .Produces(StatusCodes.Status200OK);

app.MapControllers();

app.Run();

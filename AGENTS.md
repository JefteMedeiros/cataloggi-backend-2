# Repository Guidelines

## Project Structure & Module Organization
This repository contains a single .NET API solution.

- `cataloggi-backend-2.sln`: solution entry point.
- `cataloggi-backend-2/Program.cs`: application startup, middleware, and dependency registration.
- `cataloggi-backend-2/Endpoints/`: minimal API endpoint definitions by feature.
- `cataloggi-backend-2/Services/`: business logic.
- `cataloggi-backend-2/Repositories/`: data access abstractions and EF Core persistence.
- `cataloggi-backend-2/Data/AppDbContext.cs`: EF Core database context.
- `cataloggi-backend-2/Models/`: domain entities such as `Category` and `Item`.
- `cataloggi-backend-2/DTOs/`: request and response DTOs grouped by feature.
- `cataloggi-backend-2/Migrations/`: EF Core migrations.

Add new feature code in the same layered style: endpoint, service, repository, model, and DTO files grouped by concern.

## Build, Test, and Development Commands
Run commands from the repository root.

- `dotnet restore cataloggi-backend-2.sln`: restore NuGet dependencies.
- `dotnet build cataloggi-backend-2.sln -c Release`: compile the solution in Release mode.
- `dotnet run --project cataloggi-backend-2/cataloggi-backend-2.csproj`: start the API locally.
- `dotnet watch --project cataloggi-backend-2/cataloggi-backend-2.csproj run`: run with hot reload during development.
- `dotnet test`: run tests when a test project is present.

Use `cataloggi-backend-2/cataloggi-backend-2.http` for local API request examples.

## Coding Style & Naming Conventions
Use standard C# conventions: 4-space indentation, nullable-aware code, `PascalCase` for public types and members, and `camelCase` for locals and parameters.

Keep names explicit and feature-oriented. Examples: `CreateItemDto`, `UpdateCategoryDto`, `ItemSummaryDto`, `IItemRepository`, `CategoryService`. Keep dependency registration centralized in `Program.cs`; place feature behavior in dedicated files.

## Testing Guidelines
There is currently no test project. Add tests under `tests/`, for example `tests/Cataloggi.Backend.Tests`.

Preferred stack: xUnit and FluentAssertions. Name test files `<ClassName>Tests.cs` and test methods `MethodName_ShouldExpectedBehavior_WhenCondition`. Run `dotnet test` before opening a pull request. For API changes, include representative request examples or test coverage for validation, success, and error paths.

## Commit & Pull Request Guidelines
Git history follows Conventional Commit-style messages, such as `feat: item summaries endpoint` and `chore: stronger dto validation`. Use concise prefixes like `feat:`, `fix:`, `chore:`, and `refactor:`.

Pull requests should include a short behavior summary, linked issue when applicable, test evidence, and any migration or configuration notes. Include API examples when endpoint behavior changes.

## Security & Configuration Tips
Do not commit secrets in `appsettings*.json`. Keep connection strings and credentials in user secrets, environment variables, or deployment-specific configuration. Validate incoming DTOs before persistence, and avoid committing generated local database files.

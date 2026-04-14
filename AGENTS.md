# Repository Guidelines

## Project Structure & Module Organization
This repository is a .NET solution with one API project:
- `cataloggi-backend-2.sln`: solution entry point.
- `cataloggi-backend-2/Program.cs`: app startup and dependency wiring.
- `cataloggi-backend-2/AppDbContext/`: Entity Framework Core `DbContext`.
- `cataloggi-backend-2/Models/`: domain entities (`Item`, `Category`).
- `cataloggi-backend-2/DTOs/`: request/response DTOs grouped by feature.
- `cataloggi-backend-2/appsettings*.json`: environment-specific configuration.

Keep new feature files grouped by concern (for example `DTOs/Order/` and `Models/Order.cs`).

## Build, Test, and Development Commands
Run commands from repository root unless noted.
- `dotnet restore cataloggi-backend-2.sln`: restore NuGet packages.
- `dotnet build cataloggi-backend-2.sln -c Release`: compile solution.
- `dotnet run --project .\cataloggi-backend-2\cataloggi-backend-2.csproj`: start API locally.
- `dotnet watch --project .\cataloggi-backend-2\cataloggi-backend-2.csproj run`: run with hot reload.
- `dotnet test`: run tests (currently no test project in this checkout).

## Coding Style & Naming Conventions
- Use C# defaults: 4-space indentation, UTF-8, nullable reference types enabled.
- Use `PascalCase` for classes, methods, and public properties.
- Use `camelCase` for local variables and parameters.
- Keep DTO names explicit: `CreateXDto`, `UpdateXDto`, `XDto`, `XSummaryDto`.
- Keep startup registration in `Program.cs`; move feature logic to dedicated files/folders.

## Testing Guidelines
There is no test project yet. Add tests under a top-level `tests/` directory (for example `tests/Cataloggi.Backend.Tests`).
- Preferred stack: `xUnit` + `FluentAssertions`.
- Name files as `<ClassName>Tests.cs`.
- Name test methods as `MethodName_ShouldExpectedBehavior_WhenCondition`.
- Run all tests with `dotnet test` before opening a PR.

## Commit & Pull Request Guidelines
Git history is not available in this exported workspace, so follow Conventional Commits:
- `feat: add item filtering endpoint`
- `fix: validate category id on create item`

For PRs, include:
- concise summary of behavior changes,
- linked issue (`Closes #123`) when applicable,
- test evidence (`dotnet test` output or API call examples),
- config or migration notes if data model/config changed.

## Security & Configuration Tips
- Never commit secrets in `appsettings*.json`.
- Keep connection strings in environment-specific settings or user secrets.
- Validate all incoming DTOs before persistence.

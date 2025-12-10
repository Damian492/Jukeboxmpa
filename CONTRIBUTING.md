# Contributing Guidelines

## Purpose
This file documents project-level guidelines for adding authentication and expanding the database schema. It ensures consistent implementation of login, migrations, and secrets handling.

## Authentication
- Use ASP.NET Core Identity for user authentication and authorization.
- Prefer the built-in Identity Razor Pages UI for login/register/forgot password flows.
- Use `IdentityUser` unless a custom user model is required; if custom, inherit from `IdentityUser` and add properties.
- Do not store secrets (like SMTP credentials) in source control. Use __User Secrets__ for local development and environment variables or secure secret stores in production.

## Database & Migrations
- Use Entity Framework Core migrations to change the schema.
- Add migrations with __dotnet ef migrations add <Name>__ and apply with __dotnet ef database update__ (or use the Package Manager Console equivalents: __Add-Migration <Name>__ and __Update-Database__).
- Commit migration files to the repository. Each migration must have a clear name and description.
- When adding Identity, create an initial migration (e.g., `IdentityInit`) and ensure it is applied before deploying.

## Coding & Naming
- Follow existing project naming and folder structure. Keep controllers, views, and Razor Pages separated per current conventions.
- DbContext should be named `ApplicationDbContext` and contain DbSet properties for domain entities.
- When extending the DbContext for Identity, inherit from `IdentityDbContext`.

## Pull Request Checklist
- Include migration files and verify `dotnet ef database update` runs locally.
- Verify login/register flows work locally and that users are persisted.
- Ensure no plaintext secrets are committed.
- Add unit or integration tests for critical authentication flows if possible.
- Update README or project documentation when login functionality is added.

## Tooling
- Use __dotnet ef__ CLI or Visual Studio's __Package Manager Console__ for migrations.
- Install necessary NuGet packages: `Microsoft.AspNetCore.Identity.EntityFrameworkCore` and `Microsoft.AspNetCore.Identity.UI` when using the Identity UI.

## Contacts
- When in doubt about schema changes or authentication choices, open a pull request and request review from project maintainers.

<!-- End of CONTRIBUTING.md -->
# Project & Task Management API

A backend API for managing projects and tasks, built with .NET 9 and Clean Architecture.

## Tech Stack

- .NET 9 / ASP.NET Core Web API
- Entity Framework Core 9 + SQL Server
- JWT Authentication + BCrypt password hashing
- FluentValidation
- Swagger / OpenAPI

## Architecture

Clean Architecture with 4 layers — dependency flows inward toward the domain core.

```
ProjectManagement.API            → Controllers, Middleware
ProjectManagement.Application     → DTOs, Interfaces, Validators
ProjectManagement.Domain         → Entities, Enums (no dependencies)
ProjectManagement.Infrastructure → DbContext, Services, Migrations
```

- **Domain** holds pure business entities with zero external references
- **Application** defines service interfaces and DTOs that Infrastructure implements
- **Infrastructure** contains all data access and external concerns (EF Core, JWT)
- **API** wires everything together via DI and hosts the HTTP endpoints

All services are registered through `DependencyInjection.cs` in the Infrastructure project. Controllers depend only on Application-layer interfaces, never on Infrastructure directly.

## Setup

1. Clone the repo and open in Visual Studio 2022 or VS Code
2. Check the connection string in `ProjectManagement.API/appsettings.json` — defaults to local SQL Server:
   ```
   Server=localhost;Database=ProjectManagementDB;Trusted_Connection=True;TrustServerCertificate=True;
   ```
3. Run the migration to create the database:
   ```
   dotnet ef database update --project ProjectManagement.Infrastructure --startup-project ProjectManagement.API
   ```
4. Start the API:
   ```
   dotnet run --project ProjectManagement.API
   ```
5. Browse Swagger at the URL shown in terminal (usually `https://localhost:5001/swagger`)

## API Endpoints

**Auth** (no token required)

| POST | `/api/auth/register` | Register |
| POST | `/api/auth/login`    | Login → JWT token |

**Projects** (Bearer token required)

| POST   | `/api/projects`       | Create project |
| GET    | `/api/projects`       | List user's projects |
| GET    | `/api/projects/{id}`  | Get by ID |
| PUT    | `/api/projects/{id}`  | Update |
| DELETE | `/api/projects/{id}`  | Delete |

**Tasks** (Bearer token required)

| POST   | `/api/tasks`                    | Create task |
| GET    | `/api/tasks/project/{projectId}` | List tasks in project |
| PUT    | `/api/tasks/{taskId}/status`     | Update status |
| DELETE | `/api/tasks/{taskId}`            | Delete |

Task status: `Todo`, `InProgress`, `Done`
Task priority: `low`, `medium`, `high`

## Quick Start

1. Call `/api/auth/register` with `{ fullName, email, password }`
2. Call `/api/auth/login` with `{ email, password }` → get token
3. Add `Authorization: Bearer <token>` header to all project/task requests
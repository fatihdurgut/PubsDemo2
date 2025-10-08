---
goal: Develop Modern Cloud-Native Web Application for Pubs Database
version: 1.0
date_created: 2025-10-07
last_updated: 2025-10-07
owner: Development Team
status: 'Planned'
tags: ['feature', 'cloud-native', 'aspnet-core', 'web-api', 'blazor', 'docker', 'architecture']
---

# Introduction

![Status: Planned](https://img.shields.io/badge/status-Planned-blue)

This implementation plan outlines the development of a modern, cloud-native web application for the classic Pubs database using C# and .NET 9. The solution will feature a RESTful Web API backend, Blazor WebAssembly frontend, containerized deployment with Docker, and support for deployment on any cloud vendor (Azure, AWS, GCP) or local environments.

## 1. Requirements & Constraints

**Database Schema Analysis:**
- **REQ-001**: Support existing Pubs database schema with 11 tables: authors, publishers, titles, titleauthor, employee, jobs, stores, sales, discounts, roysched, pub_info
- **REQ-002**: Maintain data integrity with existing foreign key relationships
- **REQ-003**: Support legacy column names (au_id, pub_id, title_id, etc.)

**Architectural Requirements:**
- **REQ-004**: Clean Architecture with separated concerns (Domain, Application, Infrastructure, API, Web)
- **REQ-005**: RESTful API following OpenAPI 3.0 specification
- **REQ-006**: Asynchronous operations for all I/O-bound work
- **REQ-007**: Entity Framework Core for ORM with migration support

**Cloud-Native Requirements:**
- **REQ-008**: Docker containerization for both API and Web frontend
- **REQ-009**: Docker Compose for local development orchestration
- **REQ-010**: Health checks and readiness probes for container orchestration
- **REQ-011**: Environment-based configuration (Development, Staging, Production)
- **REQ-012**: Support for cloud-agnostic deployment (Azure, AWS, GCP, on-premises)

**Security Requirements:**
- **SEC-001**: JWT-based authentication and authorization
- **SEC-002**: CORS configuration for frontend-backend communication
- **SEC-003**: Input validation on all API endpoints
- **SEC-004**: Secure connection strings using environment variables or secrets management
- **SEC-005**: SQL injection prevention through parameterized queries (EF Core)

**Performance Requirements:**
- **PERF-001**: Pagination support for large data sets
- **PERF-002**: Response caching where appropriate
- **PERF-003**: Async/await patterns throughout the application
- **PERF-004**: Database connection pooling

**Testing Requirements:**
- **TEST-001**: Unit tests with XUnit for business logic
- **TEST-002**: Integration tests for API endpoints
- **TEST-003**: Minimum 80% code coverage for Core and Application layers

**Documentation Requirements:**
- **DOC-001**: OpenAPI/Swagger documentation for all API endpoints
- **DOC-002**: XML documentation comments for public APIs
- **DOC-003**: README with setup and deployment instructions
- **DOC-004**: Architecture decision records (ADRs)

**Constraints:**
- **CON-001**: Must work with SQL Server 2019+ and Azure SQL Database
- **CON-002**: Must use .NET 9 and C# 13
- **CON-003**: Frontend must be modern SPA (Blazor WebAssembly preferred)
- **CON-004**: CI/CD ready architecture

**Guidelines:**
- **GUD-001**: Follow SOLID principles throughout the codebase
- **GUD-002**: Use Repository pattern for data access abstraction
- **GUD-003**: Implement MediatR for CQRS pattern
- **GUD-004**: Use FluentValidation for input validation
- **GUD-005**: Follow Microsoft's REST API guidelines

**Patterns:**
- **PAT-001**: Clean Architecture (Domain, Application, Infrastructure, Presentation)
- **PAT-002**: Repository Pattern for data access
- **PAT-003**: CQRS with MediatR
- **PAT-004**: Factory Pattern for complex object creation
- **PAT-005**: Dependency Injection throughout

## 2. Implementation Steps

### Implementation Phase 1: Project Setup & Infrastructure

- GOAL-001: Create solution structure with clean architecture layers

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-001 | Create solution file: `dotnet new sln -n PubsApp` | |  |
| TASK-002 | Create Core project (Domain Models): `dotnet new classlib -n PubsApp.Core` | |  |
| TASK-003 | Create Application project (Business Logic): `dotnet new classlib -n PubsApp.Application` | |  |
| TASK-004 | Create Infrastructure project (Data Access): `dotnet new classlib -n PubsApp.Infrastructure` | |  |
| TASK-005 | Create API project: `dotnet new webapi -n PubsApp.API` | |  |
| TASK-006 | Create Blazor WebAssembly project: `dotnet new blazorwasm -n PubsApp.Web` | |  |
| TASK-007 | Create Test projects: `dotnet new xunit -n PubsApp.Tests.Unit` and `PubsApp.Tests.Integration` | |  |
| TASK-008 | Add all projects to solution: `dotnet sln add **/*.csproj` | |  |
| TASK-009 | Configure project references between layers | |  |

### Implementation Phase 2: Domain Layer (Core)

- GOAL-002: Define domain entities matching Pubs database schema

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-010 | Create Author entity with properties: AuthorId, LastName, FirstName, Phone, Address, City, State, Zip, Contract | |  |
| TASK-011 | Create Publisher entity with properties: PublisherId, PublisherName, City, State, Country | |  |
| TASK-012 | Create Title entity with properties: TitleId, TitleName, Type, PublisherId, Price, Advance, Royalty, YtdSales, Notes, PubDate | |  |
| TASK-013 | Create TitleAuthor entity (many-to-many relationship) | |  |
| TASK-014 | Create Employee entity with properties and Job reference | |  |
| TASK-015 | Create Store entity with properties | |  |
| TASK-016 | Create Sale entity with properties | |  |
| TASK-017 | Create Discount and RoyaltySched entities | |  |
| TASK-018 | Define IRepository<T> interface for generic repository pattern | |  |
| TASK-019 | Define domain-specific repository interfaces (IAuthorRepository, ITitleRepository, etc.) | |  |

### Implementation Phase 3: Infrastructure Layer (Data Access)

- GOAL-003: Implement Entity Framework Core data access with repository pattern

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-020 | Install EF Core packages: Microsoft.EntityFrameworkCore.SqlServer, Microsoft.EntityFrameworkCore.Design | |  |
| TASK-021 | Create PubsDbContext inheriting from DbContext | |  |
| TASK-022 | Configure DbSet<T> for all entities | |  |
| TASK-023 | Implement OnModelCreating with Fluent API to map to existing database schema (handle legacy column names) | |  |
| TASK-024 | Configure entity relationships and foreign keys | |  |
| TASK-025 | Implement GenericRepository<T> implementing IRepository<T> | |  |
| TASK-026 | Implement specific repositories (AuthorRepository, TitleRepository, PublisherRepository, etc.) | |  |
| TASK-027 | Create Unit of Work pattern implementation | |  |
| TASK-028 | Add connection string configuration support with environment variable override | |  |
| TASK-029 | Add database migrations: `dotnet ef migrations add InitialCreate` | |  |

### Implementation Phase 4: Application Layer (Business Logic)

- GOAL-004: Implement CQRS pattern with MediatR for business operations

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-030 | Install MediatR and FluentValidation packages | |  |
| TASK-031 | Create DTOs for Authors (AuthorDto, CreateAuthorDto, UpdateAuthorDto) | |  |
| TASK-032 | Create DTOs for Titles (TitleDto, CreateTitleDto, UpdateTitleDto) | |  |
| TASK-033 | Create DTOs for Publishers (PublisherDto, CreatePublisherDto, UpdatePublisherDto) | |  |
| TASK-034 | Implement Author Commands: CreateAuthorCommand, UpdateAuthorCommand, DeleteAuthorCommand | |  |
| TASK-035 | Implement Author Queries: GetAuthorByIdQuery, GetAllAuthorsQuery, SearchAuthorsQuery | |  |
| TASK-036 | Implement Title Commands and Queries | |  |
| TASK-037 | Implement Publisher Commands and Queries | |  |
| TASK-038 | Create FluentValidation validators for all Create/Update DTOs | |  |
| TASK-039 | Implement AutoMapper profiles for entity-to-DTO mapping | |  |
| TASK-040 | Create pagination helper classes (PagedResult<T>, PaginationParams) | |  |

### Implementation Phase 5: API Layer (Web API)

- GOAL-005: Build RESTful API with OpenAPI documentation and best practices

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-041 | Configure Swagger/OpenAPI in Program.cs | |  |
| TASK-042 | Set up dependency injection for all services and repositories | |  |
| TASK-043 | Configure DbContext with connection string from configuration | |  |
| TASK-044 | Add CORS policy configuration for frontend communication | |  |
| TASK-045 | Implement AuthorsController with CRUD endpoints (GET, POST, PUT, DELETE) | |  |
| TASK-046 | Implement TitlesController with CRUD endpoints and search | |  |
| TASK-047 | Implement PublishersController with CRUD endpoints | |  |
| TASK-048 | Add pagination support to GET endpoints | |  |
| TASK-049 | Implement global exception handling middleware | |  |
| TASK-050 | Add API versioning support | |  |
| TASK-051 | Implement health check endpoints (/health, /health/ready) | |  |
| TASK-052 | Configure structured logging with Serilog | |  |
| TASK-053 | Add XML documentation for Swagger UI | |  |

### Implementation Phase 6: Blazor Web Frontend

- GOAL-006: Create modern SPA frontend for data management

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-054 | Set up HttpClient service for API communication | |  |
| TASK-055 | Create AuthorService for API calls | |  |
| TASK-056 | Create TitleService for API calls | |  |
| TASK-057 | Create PublisherService for API calls | |  |
| TASK-058 | Build Authors list page with pagination | |  |
| TASK-059 | Build Author details/edit page | |  |
| TASK-060 | Build Author create page | |  |
| TASK-061 | Build Titles list page with search and filtering | |  |
| TASK-062 | Build Title details/edit page | |  |
| TASK-063 | Build Publishers list page | |  |
| TASK-064 | Implement shared components (DataGrid, Pagination, SearchBox) | |  |
| TASK-065 | Add loading indicators and error handling | |  |
| TASK-066 | Configure routing and navigation | |  |

### Implementation Phase 7: Containerization & Docker

- GOAL-007: Enable container-based deployment for cloud and local environments

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-067 | Create Dockerfile for API project (multi-stage build) | |  |
| TASK-068 | Create Dockerfile for Blazor Web project | |  |
| TASK-069 | Create docker-compose.yml for local development (API + Web + SQL Server) | |  |
| TASK-070 | Create docker-compose.prod.yml for production deployment | |  |
| TASK-071 | Add .dockerignore file | |  |
| TASK-072 | Configure environment variables for container deployment | |  |
| TASK-073 | Test Docker builds locally: `docker-compose up --build` | |  |
| TASK-074 | Document container deployment process in README | |  |

### Implementation Phase 8: Testing

- GOAL-008: Implement comprehensive test coverage

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-075 | Write unit tests for Author domain entities | |  |
| TASK-076 | Write unit tests for FluentValidation validators | |  |
| TASK-077 | Write unit tests for MediatR command handlers | |  |
| TASK-078 | Write unit tests for MediatR query handlers | |  |
| TASK-079 | Write integration tests for Authors API endpoints | |  |
| TASK-080 | Write integration tests for Titles API endpoints | |  |
| TASK-081 | Configure test database (SQL Server in Docker or In-Memory) | |  |
| TASK-082 | Add test coverage reporting | |  |
| TASK-083 | Configure CI test execution | |  |

### Implementation Phase 9: Security & Authentication

- GOAL-009: Implement JWT authentication and authorization

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-084 | Install Microsoft.AspNetCore.Authentication.JwtBearer | |  |
| TASK-085 | Configure JWT authentication in Program.cs | |  |
| TASK-086 | Create User entity and authentication tables | |  |
| TASK-087 | Implement AuthenticationService for login | |  |
| TASK-088 | Create AuthController with login/register endpoints | |  |
| TASK-089 | Add [Authorize] attributes to protected endpoints | |  |
| TASK-090 | Implement role-based authorization | |  |
| TASK-091 | Add JWT token handling in Blazor frontend | |  |
| TASK-092 | Implement authentication state provider | |  |

### Implementation Phase 10: Cloud Deployment Preparation

- GOAL-010: Prepare application for multi-cloud deployment

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-093 | Create appsettings.Development.json, appsettings.Production.json | |  |
| TASK-094 | Configure Azure App Service deployment settings | |  |
| TASK-095 | Configure AWS Elastic Beanstalk deployment settings | |  |
| TASK-096 | Configure GCP Cloud Run deployment settings | |  |
| TASK-097 | Add Azure Application Insights integration | |  |
| TASK-098 | Add AWS CloudWatch integration | |  |
| TASK-099 | Document cloud deployment procedures for each platform | |  |
| TASK-100 | Create CI/CD pipeline templates (GitHub Actions, Azure DevOps) | |  |

## 3. Alternatives

- **ALT-001**: **Frontend Choice - React or Angular instead of Blazor WebAssembly**
  - *Rejected*: Blazor provides C# full-stack development, reducing context switching and leveraging existing .NET skills
  
- **ALT-002**: **Database-First vs Code-First EF Core**
  - *Chosen*: Code-First with explicit mapping to legacy schema provides better control and maintainability
  
- **ALT-003**: **Minimal APIs instead of Controllers**
  - *Rejected*: Controllers provide better organization for larger APIs, though minimal APIs could be considered for future refactoring
  
- **ALT-004**: **gRPC instead of REST**
  - *Rejected*: REST is more widely supported and easier to consume from various clients
  
- **ALT-005**: **Microservices architecture**
  - *Rejected*: Monolithic approach is sufficient for this application size; can be refactored to microservices later if needed

## 4. Dependencies

- **DEP-001**: .NET 9 SDK
- **DEP-002**: SQL Server 2019+ or Azure SQL Database
- **DEP-003**: Docker Desktop (for containerization)
- **DEP-004**: NuGet Packages:
  - Microsoft.EntityFrameworkCore.SqlServer (9.0.0)
  - Microsoft.EntityFrameworkCore.Design (9.0.0)
  - MediatR (12.4.0)
  - AutoMapper.Extensions.Microsoft.DependencyInjection (13.0.0)
  - FluentValidation.AspNetCore (11.3.0)
  - Swashbuckle.AspNetCore (6.8.0)
  - Serilog.AspNetCore (8.0.0)
  - Microsoft.AspNetCore.Authentication.JwtBearer (9.0.0)
  - xUnit (2.9.0)
  - Moq (4.20.0)
  - FluentAssertions (7.0.0)

## 5. Files

- **FILE-001**: `/src/PubsApp.Core/Entities/Author.cs` - Author domain entity
- **FILE-002**: `/src/PubsApp.Core/Entities/Title.cs` - Title domain entity
- **FILE-003**: `/src/PubsApp.Core/Entities/Publisher.cs` - Publisher domain entity
- **FILE-004**: `/src/PubsApp.Core/Interfaces/IRepository.cs` - Generic repository interface
- **FILE-005**: `/src/PubsApp.Infrastructure/Data/PubsDbContext.cs` - EF Core DbContext
- **FILE-006**: `/src/PubsApp.Infrastructure/Repositories/GenericRepository.cs` - Generic repository implementation
- **FILE-007**: `/src/PubsApp.Application/DTOs/AuthorDto.cs` - Author data transfer objects
- **FILE-008**: `/src/PubsApp.Application/Commands/CreateAuthorCommand.cs` - Create author command
- **FILE-009**: `/src/PubsApp.Application/Queries/GetAuthorsQuery.cs` - Get authors query
- **FILE-010**: `/src/PubsApp.Application/Validators/CreateAuthorValidator.cs` - FluentValidation validator
- **FILE-011**: `/src/PubsApp.API/Controllers/AuthorsController.cs` - Authors API controller
- **FILE-012**: `/src/PubsApp.API/Program.cs` - API startup and configuration
- **FILE-013**: `/src/PubsApp.API/appsettings.json` - Application configuration
- **FILE-014**: `/src/PubsApp.Web/Pages/Authors/AuthorList.razor` - Authors list page
- **FILE-015**: `/src/PubsApp.Web/Services/AuthorService.cs` - Author API client service
- **FILE-016**: `/Dockerfile.api` - API Docker container definition
- **FILE-017**: `/Dockerfile.web` - Web Docker container definition
- **FILE-018**: `/docker-compose.yml` - Docker Compose orchestration
- **FILE-019**: `/README.md` - Project documentation
- **FILE-020**: `/.github/workflows/ci-cd.yml` - GitHub Actions CI/CD pipeline

## 6. Testing

- **TEST-001**: Unit tests for Author entity validation
- **TEST-002**: Unit tests for CreateAuthorCommand handler
- **TEST-003**: Unit tests for GetAuthorsQuery handler
- **TEST-004**: Unit tests for CreateAuthorValidator
- **TEST-005**: Integration test for POST /api/authors endpoint
- **TEST-006**: Integration test for GET /api/authors endpoint with pagination
- **TEST-007**: Integration test for GET /api/authors/{id} endpoint
- **TEST-008**: Integration test for PUT /api/authors/{id} endpoint
- **TEST-009**: Integration test for DELETE /api/authors/{id} endpoint
- **TEST-010**: End-to-end test for complete author creation workflow
- **TEST-011**: Performance test for paginated queries with large datasets
- **TEST-012**: Security test for authentication and authorization

## 7. Risks & Assumptions

**Risks:**
- **RISK-001**: Legacy database schema may have constraints or triggers not immediately visible
  - *Mitigation*: Thoroughly analyze database schema using SQL Server tools before implementation
  
- **RISK-002**: Performance issues with large datasets in titles and sales tables
  - *Mitigation*: Implement pagination, caching, and database indexing strategy early
  
- **RISK-003**: Docker container size and startup time
  - *Mitigation*: Use multi-stage builds and Alpine-based images where possible
  
- **RISK-004**: Cross-origin issues between Blazor frontend and API
  - *Mitigation*: Properly configure CORS policies from the start

**Assumptions:**
- **ASSUMPTION-001**: Database schema remains stable during development
- **ASSUMPTION-002**: SQL Server connection is available for local development
- **ASSUMPTION-003**: Users have basic knowledge of the Pubs business domain
- **ASSUMPTION-004**: Cloud deployment will use managed database services (Azure SQL, AWS RDS, etc.)
- **ASSUMPTION-005**: Application will not require real-time features (WebSocket/SignalR)

## 8. Related Specifications / Further Reading

- [Microsoft REST API Guidelines](https://github.com/microsoft/api-guidelines/blob/vNext/Guidelines.md)
- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [.NET Microservices Architecture eBook](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/)
- [Docker Best Practices for .NET](https://learn.microsoft.com/en-us/dotnet/core/docker/build-container)
- [Entity Framework Core Documentation](https://learn.microsoft.com/en-us/ef/core/)
- [Blazor Documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/)
- [MediatR Documentation](https://github.com/jbogard/MediatR/wiki)
- [FluentValidation Documentation](https://docs.fluentvalidation.net/)

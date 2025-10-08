using PubsApp.API.Middleware;
using PubsApp.Application;
using PubsApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add Application services (MediatR, AutoMapper, Validators)
builder.Services.AddApplication();

// Add Infrastructure services (DbContext, Repositories, Unit of Work)
builder.Services.AddInfrastructure(builder.Configuration);

// Add OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Pubs API",
        Version = "v1",
        Description = "A modern, cloud-native API for the classic Pubs database",
        Contact = new()
        {
            Name = "Pubs Development Team"
        }
    });
});

// Add CORS for development
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<PubsApp.Infrastructure.Data.PubsDbContext>("database");

var app = builder.Build();

// Configure the HTTP request pipeline

// Global exception handling middleware (before other middleware)
app.UseExceptionHandlingMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Pubs API v1");
        options.RoutePrefix = string.Empty; // Serve Swagger UI at root
    });
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

// Map health check endpoints
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready");

app.Run();

// Make Program class accessible to test projects
public partial class Program { }

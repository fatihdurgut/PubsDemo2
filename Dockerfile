# syntax=docker/dockerfile:1

# ============================================
# Stage 1: Build Stage
# ============================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files for dependency resolution
COPY ["PubsApp.sln", "."]
COPY ["src/PubsApp.Core/PubsApp.Core.csproj", "src/PubsApp.Core/"]
COPY ["src/PubsApp.Application/PubsApp.Application.csproj", "src/PubsApp.Application/"]
COPY ["src/PubsApp.Infrastructure/PubsApp.Infrastructure.csproj", "src/PubsApp.Infrastructure/"]
COPY ["src/PubsApp.API/PubsApp.API.csproj", "src/PubsApp.API/"]

# Restore dependencies (cached layer if project files don't change)
RUN dotnet restore "src/PubsApp.API/PubsApp.API.csproj"

# Copy all source code
COPY . .

# Build the application
WORKDIR "/src/src/PubsApp.API"
RUN dotnet build "PubsApp.API.csproj" -c Release -o /app/build --no-restore

# ============================================
# Stage 2: Publish Stage
# ============================================
FROM build AS publish
RUN dotnet publish "PubsApp.API.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

# ============================================
# Stage 3: Final Runtime Stage
# ============================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

# Set working directory
WORKDIR /app

# Create a non-root user for security
RUN groupadd -r appuser && useradd -r -g appuser appuser

# Copy published application from publish stage
COPY --from=publish /app/publish .

# Change ownership to non-root user
RUN chown -R appuser:appuser /app

# Switch to non-root user
USER appuser

# Expose ports
EXPOSE 8080
EXPOSE 8081

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check configuration
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl --fail http://localhost:8080/health || exit 1

# Set the entry point
ENTRYPOINT ["dotnet", "PubsApp.API.dll"]

# Labels for metadata
LABEL maintainer="Pubs Development Team"
LABEL description="Pubs API - Modern Cloud-Native Web Application"
LABEL version="1.0.0"

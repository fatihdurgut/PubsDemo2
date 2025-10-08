# Quick Start Guide - Pubs Application

## Prerequisites
- .NET 9.0 SDK
- Docker Desktop (for containerized deployment)
- SQL Server (local or Docker)

## Running Locally (Development)

### Option 1: Using Visual Studio / VS Code

#### Step 1: Start the API
```powershell
cd src/PubsApp.API
dotnet run
```
The API will start at: **https://localhost:7080**

#### Step 2: Start the Blazor WebAssembly App (in a new terminal)
```powershell
cd src/PubsApp.Web
dotnet run
```
The Web app will start at: **https://localhost:7183**

#### Step 3: Access the Application
Open your browser and navigate to: **https://localhost:7183**

### Option 2: Using Docker Compose

#### Step 1: Build and Start All Services
```powershell
docker-compose up --build
```

This will start:
- SQL Server on port **1433**
- API on port **5159** (http://localhost:5159)
- Web UI on port **8081** (http://localhost:8081)

#### Step 2: Access the Application
Open your browser and navigate to: **http://localhost:8081**

#### Step 3: Stop All Services
```powershell
docker-compose down
```

To also remove volumes:
```powershell
docker-compose down -v
```

## Application Features

### Home Page
- Dashboard with statistics for Authors, Titles, and Publishers
- Quick action links to create new records

### Authors Management
- **List** (`/authors`): View all authors with pagination and search
- **Details** (`/authors/{id}`): View author information and their titles
- **Edit** (`/authors/edit/{id}`): Update author information
- **Create** (`/authors/create`): Add a new author

### Titles Management
- **List** (`/titles`): View all titles with pagination and search
- **Details** (`/titles/{id}`): View title information, publisher, and authors
- **Edit** (`/titles/edit/{id}`): Update title information
- **Create** (`/titles/create`): Add a new title

### Publishers Management
- **List** (`/publishers`): View all publishers with pagination and search
- **Details** (`/publishers/{id}`): View publisher information and their titles
- **Edit** (`/publishers/edit/{id}`): Update publisher information
- **Create** (`/publishers/create`): Add a new publisher

## Common Operations

### Creating a New Author
1. Navigate to **Authors** page
2. Click **"New Author"** button
3. Fill in the form:
   - Author ID (format: XXX-XX-XXXX)
   - First Name, Last Name (required)
   - Phone (required)
   - Address, City, State, ZIP (optional)
   - Contract checkbox
4. Click **"Create Author"**

### Creating a New Title
1. Navigate to **Titles** page
2. Click **"New Title"** button
3. Fill in the form:
   - Title ID (max 6 characters)
   - Title Name (required)
   - Type (select from dropdown)
   - Publisher (select from dropdown)
   - Published Date (required)
   - Price, Advance, Royalty, YTD Sales (optional)
   - Notes (optional, max 200 chars)
4. Click **"Create Title"**

### Creating a New Publisher
1. Navigate to **Publishers** page
2. Click **"New Publisher"** button
3. Fill in the form:
   - Publisher ID (exactly 4 characters)
   - Publisher Name
   - City, State, Country
4. Click **"Create Publisher"**

### Searching
1. Go to any list page (Authors, Titles, or Publishers)
2. Enter search term in the search box
3. Press **Enter** or click the search icon
4. Click the **X** button to clear search

### Pagination
- Use **Previous** and **Next** buttons to navigate pages
- Current page and total items displayed at the bottom

## API Endpoints

### Authors
- `GET /api/authors?pageNumber=1&pageSize=10` - Get paginated authors
- `GET /api/authors/{id}` - Get author by ID
- `GET /api/authors/search?searchTerm={term}` - Search authors
- `POST /api/authors` - Create new author
- `PUT /api/authors/{id}` - Update author
- `DELETE /api/authors/{id}` - Delete author

### Titles
- `GET /api/titles?pageNumber=1&pageSize=10` - Get paginated titles
- `GET /api/titles/{id}` - Get title by ID
- `GET /api/titles/search?searchTerm={term}` - Search titles
- `POST /api/titles` - Create new title
- `PUT /api/titles/{id}` - Update title
- `DELETE /api/titles/{id}` - Delete title

### Publishers
- `GET /api/publishers?pageNumber=1&pageSize=10` - Get paginated publishers
- `GET /api/publishers/{id}` - Get publisher by ID
- `GET /api/publishers/search?searchTerm={term}` - Search publishers
- `POST /api/publishers` - Create new publisher
- `PUT /api/publishers/{id}` - Update publisher
- `DELETE /api/publishers/{id}` - Delete publisher

## Troubleshooting

### API Connection Error
**Problem**: "Error loading data" messages in the Web UI

**Solutions**:
1. Verify the API is running on https://localhost:7080
2. Check the API base address in `src/PubsApp.Web/wwwroot/appsettings.json`
3. Check browser console for CORS errors
4. Verify CORS is enabled in API `Program.cs`

### Build Errors
**Problem**: Compilation errors in Blazor project

**Solutions**:
1. Clean and rebuild the solution:
   ```powershell
   dotnet clean
   dotnet build
   ```
2. Restore NuGet packages:
   ```powershell
   dotnet restore
   ```

### Docker Issues
**Problem**: Containers fail to start

**Solutions**:
1. Check Docker Desktop is running
2. Check ports 1433, 5159, and 8081 are not in use
3. View container logs:
   ```powershell
   docker-compose logs pubs-api
   docker-compose logs pubs-web
   ```
4. Rebuild containers:
   ```powershell
   docker-compose build --no-cache
   ```

### Database Connection Error
**Problem**: API fails to connect to database

**Solutions**:
1. Verify SQL Server is running
2. Check connection string in `appsettings.Development.json`
3. Run database migrations:
   ```powershell
   cd src/PubsApp.API
   dotnet ef database update
   ```

## Development Tips

### Hot Reload
The Blazor WebAssembly app supports hot reload. Changes to `.razor` files will automatically refresh the browser.

### Debugging
- **API**: Use Visual Studio debugger or attach to process
- **Blazor**: Use browser DevTools (F12)
  - Check Console tab for JavaScript errors
  - Check Network tab for API calls
  - Use Blazor DevTools extension

### Code Changes
After modifying code:
1. Stop running applications (Ctrl+C)
2. Rebuild: `dotnet build`
3. Restart applications

### Database Seeding
The database is automatically seeded with sample data on first run. To re-seed:
1. Drop the database
2. Run migrations: `dotnet ef database update`
3. Restart the API

## Next Steps

### Phase 9: Security & Authentication
- Add user registration and login
- Implement JWT authentication
- Protect API endpoints with authorization
- Add role-based access control

### Phase 10: Cloud Deployment
- Deploy to Azure/AWS/GCP
- Setup CI/CD pipelines
- Configure production environment
- Add monitoring and logging

## Support
For issues or questions, refer to:
- **Documentation**: `/docs` folder
- **Implementation Summary**: `docs/Phase6-Implementation-Summary.md`
- **API Documentation**: Swagger UI at https://localhost:7080/swagger

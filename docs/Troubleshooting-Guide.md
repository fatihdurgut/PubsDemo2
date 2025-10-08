# Troubleshooting Guide - PubsApp

## Common Issues and Solutions

### Issue 1: Frontend Cannot Connect to Backend API

**Symptoms:**
- Web application loads but shows no data
- Browser console shows network errors
- API calls fail with connection refused or timeout errors

**Root Cause:**
The `appsettings.json` file in the web app has an incorrect API base address.

**Solution:**
1. Check which port your API is running on:
   - Look at the terminal output when you start the API
   - Example: "Now listening on: http://localhost:5159"

2. Update `src/PubsApp.Web/wwwroot/appsettings.json`:
   ```json
   {
     "ApiBaseAddress": "http://localhost:5159"
   }
   ```
   **Important:** Use HTTP (not HTTPS) if your API is running on HTTP.

3. Restart your web application (Ctrl+C, then `dotnet run`)

4. Refresh your browser

---

### Issue 2: PagedResult Deserialization Error

**Symptoms:**
- Error message: "Each parameter in the deserialization constructor on type 'PubsApp.Application.Common.PagedResult`1[...]' must bind to an object property or field on deserialization"
- Home page shows "Error loading statistics"

**Root Cause:**
The `PagedResult<T>` class only had a parameterized constructor, but JSON deserialization requires a parameterless constructor.

**Solution:**
This has been fixed in the codebase. The `PagedResult<T>` class now includes both constructors:
```csharp
public PagedResult()
{
}

public PagedResult(IEnumerable<T> items, int count, int pageNumber, int pageSize)
{
    Items = items;
    TotalCount = count;
    PageNumber = pageNumber;
    PageSize = pageSize;
}
```

If you still see this error:
1. Stop the API
2. Run `dotnet build` from the solution root
3. Restart the API
4. Refresh your browser

---

### Issue 3: Publisher ID CHECK Constraint Violation

**Symptoms:**
- Error when creating a new publisher: "The INSERT statement conflicted with the CHECK constraint 'CK__publisher__pub_i__3C69FB99'"
- Status 500 error returned from API

**Root Cause:**
The pubs database has a CHECK constraint on the `pub_id` column that restricts valid publisher IDs to specific ranges.

**Valid Publisher ID Ranges:**
- **9900-9999** (recommended for new publishers)
- 0736-0877 (existing range)
- 1389, 1622, 1756 (specific existing publishers)

**Solution:**
When creating a new publisher, use an ID in the **9900-9999** range:
- ✅ Good: 9900, 9901, 9950, 9999
- ❌ Bad: 1234, 5555, 8888

The UI has been updated to suggest valid IDs with the placeholder "9900" and help text.

---

### Issue 4: CORS Errors in Browser

**Symptoms:**
- Browser console shows CORS policy errors
- Requests blocked by browser

**Root Cause:**
The API needs to allow requests from the web application origin.

**Solution:**
The API is already configured with CORS support in `Program.cs`:
```csharp
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

If you're still seeing CORS errors:
1. Check that `app.UseCors()` is called in the middleware pipeline
2. Ensure the API is running on the expected port
3. Clear your browser cache and refresh

---

## How to Start the Application

### Option 1: Run Locally (Development)

**Terminal 1 - API:**
```powershell
cd src/PubsApp.API
dotnet run
```
Wait for: "Now listening on: http://localhost:5159"

**Terminal 2 - Web:**
```powershell
cd src/PubsApp.Web
dotnet run
```
Wait for the web app URL (usually https://localhost:7183)

**Browser:**
Navigate to the web app URL shown in Terminal 2

---

### Option 2: Run with Docker Compose

```powershell
docker-compose up --build
```

Access the application:
- Web App: http://localhost:8081
- API: http://localhost:5159

---

## Testing the Fix

### Test 1: Home Page Statistics
1. Navigate to the home page (/)
2. You should see three cards showing counts for Authors, Titles, and Publishers
3. If you see "Error loading statistics", check Issue 2 above

### Test 2: Create a Publisher
1. Navigate to Publishers → Create Publisher
2. Fill in the form:
   - **Publisher ID:** 9900 (or any ID in 9900-9999 range)
   - **Publisher Name:** Test Publisher
   - **City:** San Francisco
   - **State:** CA
   - **Country:** USA
3. Click "Create Publisher"
4. You should be redirected to the publisher details page

### Test 3: View Publishers List
1. Navigate to Publishers
2. You should see a paginated list of all publishers
3. Try the search functionality
4. Click on a publisher to view details

---

## Database Schema Notes

### Publisher Table Schema
```sql
CREATE TABLE publishers (
    pub_id      char(4)     NOT NULL,
    pub_name    varchar(40) NULL,
    city        varchar(20) NULL,
    state       char(2)     NULL,
    country     varchar(30) NULL,
    CONSTRAINT PK_publishers PRIMARY KEY (pub_id),
    CONSTRAINT CK_pub_id CHECK (pub_id IN ('1389', '1622', '1756', '9952', '9999') 
                                 OR pub_id LIKE '[0-9][0-9][0-9][0-9]')
)
```

The CHECK constraint allows:
- Specific publisher IDs: 1389, 1622, 1756, 9952, 9999
- Any 4-digit number (but in practice, use 9900-9999 to avoid conflicts)

---

## Additional Resources

- **QUICKSTART.md** - Quick start guide for running the application
- **Phase6-Implementation-Summary.md** - Comprehensive implementation details
- **Phase6-Completion-Summary.md** - Phase 6 completion summary

---

## Getting Help

If you encounter issues not covered in this guide:

1. Check the API logs in the terminal
2. Check the browser console for JavaScript errors
3. Verify database connectivity
4. Ensure all required services are running (SQL Server, API, Web)

Common commands:
```powershell
# Check if SQL Server is running
docker ps | grep sql

# Rebuild the solution
dotnet build PubsApp.sln

# Clean and rebuild
dotnet clean
dotnet build

# Check for compilation errors
dotnet build --no-incremental
```

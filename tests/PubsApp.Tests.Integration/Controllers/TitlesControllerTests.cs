using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using PubsApp.Application.DTOs;
using PubsApp.Tests.Integration.Infrastructure;
using Xunit;

namespace PubsApp.Tests.Integration.Controllers;

public class TitlesControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public TitlesControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    #region GET /api/titles (Paginated List)

    [Fact]
    public async Task GetAllTitles_ShouldReturnPagedResult()
    {
        // Arrange - Create test data first
        await SeedTestTitlesAsync();

        // Act
        var response = await _client.GetAsync("/api/titles?pageNumber=1&pageSize=5");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("\"items\"", content.ToLower());
        Assert.Contains("\"totalCount\"", content.ToLower());
    }

    [Fact]
    public async Task GetAllTitles_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        await SeedTestTitlesAsync();

        // Act
        var response = await _client.GetAsync("/api/titles?pageNumber=2&pageSize=2");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("\"pageNumber\":2", content.ToLower().Replace(" ", ""));
    }

    #endregion

    #region GET /api/titles/{id}

    [Fact]
    public async Task GetTitleById_WhenTitleExists_ShouldReturnTitle()
    {
        // Arrange
        var testTitleId = "IT0001";
        await SeedSingleTestTitleAsync(testTitleId);

        // Act
        var response = await _client.GetAsync($"/api/titles/{testTitleId}");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains(testTitleId, content);
    }

    [Fact]
    public async Task GetTitleById_WhenTitleNotFound_ShouldReturn404()
    {
        // Act
        var response = await _client.GetAsync("/api/titles/XX9999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region GET /api/titles/{id}/details

    [Fact]
    public async Task GetTitleWithDetails_WhenTitleExists_ShouldReturnTitleWithPublisher()
    {
        // Arrange
        var testTitleId = "IT0002";
        await SeedSingleTestTitleAsync(testTitleId);

        // Act
        var response = await _client.GetAsync($"/api/titles/{testTitleId}/details");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains(testTitleId, content);
        // Should include publisher information
        Assert.Contains("publisher", content.ToLower());
    }

    #endregion

    #region GET /api/titles/search

    [Fact]
    public async Task SearchTitles_WithSearchTerm_ShouldReturnMatchingTitles()
    {
        // Arrange
        await SeedTestTitlesAsync();

        // Act
        var response = await _client.GetAsync("/api/titles/search?searchTerm=Integration");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Integration", content);
    }

    [Fact]
    public async Task SearchTitles_WithNoMatches_ShouldReturnEmptyList()
    {
        // Arrange
        await SeedTestTitlesAsync();

        // Act
        var response = await _client.GetAsync("/api/titles/search?searchTerm=NonExistentBook123");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var titles = JsonSerializer.Deserialize<List<TitleDto>>(content, _jsonOptions);
        Assert.NotNull(titles);
        Assert.Empty(titles);
    }

    #endregion

    #region POST /api/titles (Create)

    [Fact]
    public async Task CreateTitle_WithValidData_ShouldReturnCreatedTitle()
    {
        // Arrange
        await SeedTestPublisherAsync("1234");
        
        var newTitle = new CreateTitleDto
        {
            TitleId = "IT9999",
            TitleName = "New Integration Test Book",
            Type = "business",
            PublisherId = "1234",
            Price = 29.99m,
            Royalty = 15
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/titles", newTitle);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var createdTitle = await response.Content.ReadFromJsonAsync<TitleDto>(_jsonOptions);
        Assert.NotNull(createdTitle);
        Assert.Equal(newTitle.TitleId, createdTitle.TitleId);
        Assert.Equal(newTitle.TitleName, createdTitle.TitleName);
    }

    [Fact]
    public async Task CreateTitle_WithInvalidData_ShouldReturnBadRequest()
    {
        // Arrange - Missing required TitleId
        var invalidTitle = new CreateTitleDto
        {
            TitleId = "", // Invalid
            TitleName = "Test Book",
            Type = "business"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/titles", invalidTitle);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateTitle_WithDuplicateId_ShouldReturnBadRequest()
    {
        // Arrange
        var existingTitleId = "IT0003";
        await SeedSingleTestTitleAsync(existingTitleId);

        var duplicateTitle = new CreateTitleDto
        {
            TitleId = existingTitleId,
            TitleName = "Duplicate Book",
            Type = "business",
            PublisherId = "1234"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/titles", duplicateTitle);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region PUT /api/titles/{id} (Update)

    [Fact]
    public async Task UpdateTitle_WithValidData_ShouldReturnUpdatedTitle()
    {
        // Arrange
        var titleId = "IT0004";
        await SeedSingleTestTitleAsync(titleId);

        var updateDto = new UpdateTitleDto
        {
            TitleName = "Updated Integration Test Title",
            Price = 39.99m
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/titles/{titleId}", updateDto);

        // Assert
        response.EnsureSuccessStatusCode();
        var updatedTitle = await response.Content.ReadFromJsonAsync<TitleDto>(_jsonOptions);
        Assert.NotNull(updatedTitle);
        Assert.Equal(updateDto.TitleName, updatedTitle.TitleName);
    }

    [Fact]
    public async Task UpdateTitle_WhenTitleNotFound_ShouldReturn404()
    {
        // Arrange
        var updateDto = new UpdateTitleDto
        {
            TitleName = "Updated Title"
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/titles/XX9999", updateDto);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region DELETE /api/titles/{id}

    [Fact]
    public async Task DeleteTitle_WhenTitleExists_ShouldReturnNoContent()
    {
        // Arrange
        var titleId = "IT0005";
        await SeedSingleTestTitleAsync(titleId);

        // Act
        var response = await _client.DeleteAsync($"/api/titles/{titleId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify it's deleted
        var getResponse = await _client.GetAsync($"/api/titles/{titleId}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteTitle_WhenTitleNotFound_ShouldReturn404()
    {
        // Act
        var response = await _client.DeleteAsync("/api/titles/XX9999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Helper Methods

    private async Task SeedTestPublisherAsync(string publisherId)
    {
        var publisher = new CreatePublisherDto
        {
            PublisherId = publisherId,
            PublisherName = "Test Publisher",
            City = "Test City",
            State = "CA",
            Country = "USA"
        };

        await _client.PostAsJsonAsync("/api/publishers", publisher);
    }

    private async Task SeedSingleTestTitleAsync(string titleId)
    {
        await SeedTestPublisherAsync("1234");

        var title = new CreateTitleDto
        {
            TitleId = titleId,
            TitleName = $"Integration Test Book {titleId}",
            Type = "business",
            PublisherId = "1234",
            Price = 19.99m,
            Royalty = 10
        };

        await _client.PostAsJsonAsync("/api/titles", title);
    }

    private async Task SeedTestTitlesAsync()
    {
        await SeedTestPublisherAsync("1234");

        var titles = new[]
        {
            new CreateTitleDto
            {
                TitleId = "IT1001",
                TitleName = "Integration Testing Guide",
                Type = "business",
                PublisherId = "1234",
                Price = 29.99m
            },
            new CreateTitleDto
            {
                TitleId = "IT1002",
                TitleName = "Advanced Integration Patterns",
                Type = "business",
                PublisherId = "1234",
                Price = 39.99m
            },
            new CreateTitleDto
            {
                TitleId = "IT1003",
                TitleName = "Web API Testing",
                Type = "business",
                PublisherId = "1234",
                Price = 24.99m
            }
        };

        foreach (var title in titles)
        {
            await _client.PostAsJsonAsync("/api/titles", title);
        }
    }

    #endregion
}

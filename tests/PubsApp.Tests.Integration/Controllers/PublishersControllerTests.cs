using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using PubsApp.Application.DTOs;
using PubsApp.Tests.Integration.Infrastructure;
using Xunit;

namespace PubsApp.Tests.Integration.Controllers;

public class PublishersControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public PublishersControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    #region GET /api/publishers (Paginated List)

    [Fact]
    public async Task GetAllPublishers_ShouldReturnPagedResult()
    {
        // Arrange
        await SeedTestPublishersAsync();

        // Act
        var response = await _client.GetAsync("/api/publishers?pageNumber=1&pageSize=5");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("\"items\"", content.ToLower());
        Assert.Contains("\"totalCount\"", content.ToLower());
    }

    [Fact]
    public async Task GetAllPublishers_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        await SeedTestPublishersAsync();

        // Act
        var response = await _client.GetAsync("/api/publishers?pageNumber=2&pageSize=2");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("\"pageNumber\":2", content.ToLower().Replace(" ", ""));
    }

    #endregion

    #region GET /api/publishers/{id}

    [Fact]
    public async Task GetPublisherById_WhenPublisherExists_ShouldReturnPublisher()
    {
        // Arrange
        var testPublisherId = "9001";
        await SeedSingleTestPublisherAsync(testPublisherId, "Integration Test Publisher");

        // Act
        var response = await _client.GetAsync($"/api/publishers/{testPublisherId}");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains(testPublisherId, content);
    }

    [Fact]
    public async Task GetPublisherById_WhenPublisherNotFound_ShouldReturn404()
    {
        // Act
        var response = await _client.GetAsync("/api/publishers/9999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region GET /api/publishers/{id}/details

    [Fact]
    public async Task GetPublisherWithDetails_WhenPublisherExists_ShouldReturnPublisherWithTitles()
    {
        // Arrange
        var testPublisherId = "9002";
        await SeedSingleTestPublisherAsync(testPublisherId, "Detailed Publisher");

        // Act
        var response = await _client.GetAsync($"/api/publishers/{testPublisherId}/details");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains(testPublisherId, content);
        // Should include title count information
        Assert.Contains("titleCount", content.ToLower());
    }

    #endregion

    #region GET /api/publishers/search

    [Fact]
    public async Task SearchPublishers_WithSearchTerm_ShouldReturnMatchingPublishers()
    {
        // Arrange
        await SeedTestPublishersAsync();

        // Act
        var response = await _client.GetAsync("/api/publishers/search?searchTerm=Integration");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Integration", content);
    }

    [Fact]
    public async Task SearchPublishers_WithNoMatches_ShouldReturnEmptyList()
    {
        // Arrange
        await SeedTestPublishersAsync();

        // Act
        var response = await _client.GetAsync("/api/publishers/search?searchTerm=NonExistentPublisher123");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var publishers = JsonSerializer.Deserialize<List<PublisherDto>>(content, _jsonOptions);
        Assert.NotNull(publishers);
        Assert.Empty(publishers);
    }

    #endregion

    #region POST /api/publishers (Create)

    [Fact]
    public async Task CreatePublisher_WithValidData_ShouldReturnCreatedPublisher()
    {
        // Arrange
        var newPublisher = new CreatePublisherDto
        {
            PublisherId = "9100",
            PublisherName = "New Integration Test Publisher",
            City = "San Francisco",
            State = "CA",
            Country = "USA"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/publishers", newPublisher);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var createdPublisher = await response.Content.ReadFromJsonAsync<PublisherDto>(_jsonOptions);
        Assert.NotNull(createdPublisher);
        Assert.Equal(newPublisher.PublisherId, createdPublisher.PublisherId);
        Assert.Equal(newPublisher.PublisherName, createdPublisher.PublisherName);
    }

    [Fact]
    public async Task CreatePublisher_WithInvalidData_ShouldReturnBadRequest()
    {
        // Arrange - Invalid PublisherId (too short)
        var invalidPublisher = new CreatePublisherDto
        {
            PublisherId = "12", // Invalid - must be 4 digits
            PublisherName = "Test Publisher"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/publishers", invalidPublisher);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreatePublisher_WithDuplicateId_ShouldReturnBadRequest()
    {
        // Arrange
        var existingPublisherId = "9003";
        await SeedSingleTestPublisherAsync(existingPublisherId, "Existing Publisher");

        var duplicatePublisher = new CreatePublisherDto
        {
            PublisherId = existingPublisherId,
            PublisherName = "Duplicate Publisher"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/publishers", duplicatePublisher);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreatePublisher_WithInvalidState_ShouldReturnBadRequest()
    {
        // Arrange
        var publisherWithInvalidState = new CreatePublisherDto
        {
            PublisherId = "9200",
            PublisherName = "Test Publisher",
            State = "ZZ" // Invalid state code
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/publishers", publisherWithInvalidState);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region PUT /api/publishers/{id} (Update)

    [Fact]
    public async Task UpdatePublisher_WithValidData_ShouldReturnUpdatedPublisher()
    {
        // Arrange
        var publisherId = "9004";
        await SeedSingleTestPublisherAsync(publisherId, "Original Publisher");

        var updateDto = new UpdatePublisherDto
        {
            PublisherName = "Updated Publisher Name",
            City = "Boston"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/publishers/{publisherId}", updateDto);

        // Assert
        response.EnsureSuccessStatusCode();
        var updatedPublisher = await response.Content.ReadFromJsonAsync<PublisherDto>(_jsonOptions);
        Assert.NotNull(updatedPublisher);
        Assert.Equal(updateDto.PublisherName, updatedPublisher.PublisherName);
        Assert.Equal(updateDto.City, updatedPublisher.City);
    }

    [Fact]
    public async Task UpdatePublisher_WhenPublisherNotFound_ShouldReturn404()
    {
        // Arrange
        var updateDto = new UpdatePublisherDto
        {
            PublisherName = "Updated Name"
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/publishers/9999", updateDto);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdatePublisher_WithInvalidState_ShouldReturnBadRequest()
    {
        // Arrange
        var publisherId = "9005";
        await SeedSingleTestPublisherAsync(publisherId, "Test Publisher");

        var updateDto = new UpdatePublisherDto
        {
            State = "XX" // Invalid state code
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/publishers/{publisherId}", updateDto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region DELETE /api/publishers/{id}

    [Fact]
    public async Task DeletePublisher_WhenPublisherExists_ShouldReturnNoContent()
    {
        // Arrange
        var publisherId = "9006";
        await SeedSingleTestPublisherAsync(publisherId, "Publisher to Delete");

        // Act
        var response = await _client.DeleteAsync($"/api/publishers/{publisherId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify it's deleted
        var getResponse = await _client.GetAsync($"/api/publishers/{publisherId}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task DeletePublisher_WhenPublisherNotFound_ShouldReturn404()
    {
        // Act
        var response = await _client.DeleteAsync("/api/publishers/9999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Helper Methods

    private async Task SeedSingleTestPublisherAsync(string publisherId, string publisherName)
    {
        var publisher = new CreatePublisherDto
        {
            PublisherId = publisherId,
            PublisherName = publisherName,
            City = "Test City",
            State = "NY",
            Country = "USA"
        };

        await _client.PostAsJsonAsync("/api/publishers", publisher);
    }

    private async Task SeedTestPublishersAsync()
    {
        var publishers = new[]
        {
            new CreatePublisherDto
            {
                PublisherId = "9101",
                PublisherName = "Integration Test Books",
                City = "New York",
                State = "NY",
                Country = "USA"
            },
            new CreatePublisherDto
            {
                PublisherId = "9102",
                PublisherName = "Integration Publishing",
                City = "Boston",
                State = "MA",
                Country = "USA"
            },
            new CreatePublisherDto
            {
                PublisherId = "9103",
                PublisherName = "Tech Integration Press",
                City = "Seattle",
                State = "WA",
                Country = "USA"
            }
        };

        foreach (var publisher in publishers)
        {
            await _client.PostAsJsonAsync("/api/publishers", publisher);
        }
    }

    #endregion
}

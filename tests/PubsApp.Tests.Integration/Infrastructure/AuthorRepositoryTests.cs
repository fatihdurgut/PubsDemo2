using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PubsApp.Core.Interfaces;
using PubsApp.Infrastructure;
using PubsApp.Infrastructure.Data;

namespace PubsApp.Tests.Integration.Infrastructure;

/// <summary>
/// Integration tests for the Author Repository
/// </summary>
public class AuthorRepositoryTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IUnitOfWork _unitOfWork;

    public AuthorRepositoryTests()
    {
        // Setup in-memory configuration
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:PubsDatabase"] = "Server=FATIH-PC\\SQLEXPRESS;Database=pubs;Integrated Security=true;TrustServerCertificate=true;MultipleActiveResultSets=true"
            })
            .Build();

        // Setup dependency injection
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddInfrastructure(configuration);

        _serviceProvider = services.BuildServiceProvider();
        _unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAuthors()
    {
        // Act
        var authors = await _unitOfWork.Authors.GetAllAsync();

        // Assert
        Assert.NotNull(authors);
        Assert.NotEmpty(authors);
    }

    [Fact]
    public async Task SearchByNameAsync_WithValidTerm_ShouldReturnMatchingAuthors()
    {
        // Arrange
        const string searchTerm = "White";

        // Act
        var authors = await _unitOfWork.Authors.SearchByNameAsync(searchTerm);

        // Assert
        Assert.NotNull(authors);
        var authorList = authors.ToList();
        Assert.NotEmpty(authorList);
        Assert.All(authorList, author =>
            Assert.True(
                author.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                author.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            )
        );
    }

    [Fact]
    public async Task GetAuthorsWithTitlesAsync_ShouldIncludeRelatedData()
    {
        // Act
        var authors = await _unitOfWork.Authors.GetAuthorsWithTitlesAsync();

        // Assert
        Assert.NotNull(authors);
        var authorsList = authors.ToList();
        Assert.NotEmpty(authorsList);
        
        // Verify that at least one author has titles
        var authorWithTitles = authorsList.FirstOrDefault(a => a.TitleAuthors.Any());
        Assert.NotNull(authorWithTitles);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnAuthor()
    {
        // Arrange - Get a valid author ID first
        var allAuthors = await _unitOfWork.Authors.GetAllAsync();
        var firstAuthor = allAuthors.First();

        // Act
        var author = await _unitOfWork.Authors.GetByIdAsync(firstAuthor.AuthorId);

        // Assert
        Assert.NotNull(author);
        Assert.Equal(firstAuthor.AuthorId, author.AuthorId);
    }

    [Fact]
    public async Task GetAuthorsByStateAsync_WithValidState_ShouldReturnAuthorsInState()
    {
        // Arrange
        const string state = "CA";

        // Act
        var authors = await _unitOfWork.Authors.GetAuthorsByStateAsync(state);

        // Assert
        Assert.NotNull(authors);
        var authorsList = authors.ToList();
        Assert.NotEmpty(authorsList);
        Assert.All(authorsList, author => Assert.Equal(state, author.State));
    }

    public void Dispose()
    {
        _unitOfWork?.Dispose();
        _serviceProvider?.Dispose();
        GC.SuppressFinalize(this);
    }
}

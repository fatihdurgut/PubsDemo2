using PubsApp.Core.Entities;

namespace PubsApp.Core.Interfaces;

/// <summary>
/// Repository interface for Author-specific operations
/// </summary>
public interface IAuthorRepository : IRepository<Author>
{
    /// <summary>
    /// Gets authors with their associated titles
    /// </summary>
    Task<IEnumerable<Author>> GetAuthorsWithTitlesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches authors by name
    /// </summary>
    Task<IEnumerable<Author>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets authors by state
    /// </summary>
    Task<IEnumerable<Author>> GetAuthorsByStateAsync(string state, CancellationToken cancellationToken = default);
}

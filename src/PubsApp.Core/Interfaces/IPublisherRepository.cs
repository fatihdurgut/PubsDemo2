using PubsApp.Core.Entities;

namespace PubsApp.Core.Interfaces;

/// <summary>
/// Repository interface for Publisher-specific operations
/// </summary>
public interface IPublisherRepository : IRepository<Publisher>
{
    /// <summary>
    /// Gets publishers with their titles
    /// </summary>
    Task<IEnumerable<Publisher>> GetPublishersWithTitlesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches publishers by name
    /// </summary>
    Task<IEnumerable<Publisher>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets publishers by state
    /// </summary>
    Task<IEnumerable<Publisher>> GetPublishersByStateAsync(string state, CancellationToken cancellationToken = default);
}

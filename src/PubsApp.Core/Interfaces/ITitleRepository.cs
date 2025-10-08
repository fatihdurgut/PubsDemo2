using PubsApp.Core.Entities;

namespace PubsApp.Core.Interfaces;

/// <summary>
/// Repository interface for Title-specific operations
/// </summary>
public interface ITitleRepository : IRepository<Title>
{
    /// <summary>
    /// Gets titles with their authors and publisher
    /// </summary>
    Task<IEnumerable<Title>> GetTitlesWithDetailsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches titles by name or notes
    /// </summary>
    Task<IEnumerable<Title>> SearchTitlesAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets titles by type
    /// </summary>
    Task<IEnumerable<Title>> GetTitlesByTypeAsync(string type, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets titles by publisher
    /// </summary>
    Task<IEnumerable<Title>> GetTitlesByPublisherAsync(string publisherId, CancellationToken cancellationToken = default);
}

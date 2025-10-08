using Microsoft.EntityFrameworkCore;
using PubsApp.Core.Entities;
using PubsApp.Core.Interfaces;
using PubsApp.Infrastructure.Data;

namespace PubsApp.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Publisher entity
/// </summary>
public class PublisherRepository : GenericRepository<Publisher>, IPublisherRepository
{
    public PublisherRepository(PubsDbContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Publisher>> GetPublishersWithTitlesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Titles)
            .OrderBy(p => p.PublisherName)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Publisher>> SearchByNameAsync(
        string searchTerm,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllAsync(cancellationToken);

        var lowerSearchTerm = searchTerm.ToLower();
        return await _dbSet
            .Where(p => p.PublisherName != null && p.PublisherName.ToLower().Contains(lowerSearchTerm))
            .OrderBy(p => p.PublisherName)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Publisher>> GetPublishersByStateAsync(
        string state,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(state))
            return Enumerable.Empty<Publisher>();

        return await _dbSet
            .Where(p => p.State == state.ToUpper())
            .OrderBy(p => p.City)
            .ThenBy(p => p.PublisherName)
            .ToListAsync(cancellationToken);
    }
}

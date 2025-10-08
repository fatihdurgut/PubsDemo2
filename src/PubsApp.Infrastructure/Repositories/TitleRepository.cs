using Microsoft.EntityFrameworkCore;
using PubsApp.Core.Entities;
using PubsApp.Core.Interfaces;
using PubsApp.Infrastructure.Data;

namespace PubsApp.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Title entity
/// </summary>
public class TitleRepository : GenericRepository<Title>, ITitleRepository
{
    public TitleRepository(PubsDbContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Title>> GetTitlesWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Publisher)
            .Include(t => t.TitleAuthors)
            .ThenInclude(ta => ta.Author)
            .OrderBy(t => t.TitleName)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Title>> SearchTitlesAsync(
        string searchTerm,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllAsync(cancellationToken);

        var lowerSearchTerm = searchTerm.ToLower();
        return await _dbSet
            .Include(t => t.Publisher)
            .Where(t => t.TitleName.ToLower().Contains(lowerSearchTerm) ||
                       (t.Notes != null && t.Notes.ToLower().Contains(lowerSearchTerm)))
            .OrderBy(t => t.TitleName)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Title>> GetTitlesByTypeAsync(
        string type,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(type))
            return Enumerable.Empty<Title>();

        return await _dbSet
            .Include(t => t.Publisher)
            .Where(t => t.Type == type)
            .OrderBy(t => t.TitleName)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Title>> GetTitlesByPublisherAsync(
        string publisherId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(publisherId))
            return Enumerable.Empty<Title>();

        return await _dbSet
            .Include(t => t.Publisher)
            .Where(t => t.PublisherId == publisherId)
            .OrderBy(t => t.TitleName)
            .ToListAsync(cancellationToken);
    }
}

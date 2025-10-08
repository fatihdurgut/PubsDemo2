using Microsoft.EntityFrameworkCore;
using PubsApp.Core.Entities;
using PubsApp.Core.Interfaces;
using PubsApp.Infrastructure.Data;

namespace PubsApp.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Author entity
/// </summary>
public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
{
    public AuthorRepository(PubsDbContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Author>> GetAuthorsWithTitlesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(a => a.TitleAuthors)
            .ThenInclude(ta => ta.Title)
            .ThenInclude(t => t.Publisher)
            .OrderBy(a => a.LastName)
            .ThenBy(a => a.FirstName)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Author>> SearchByNameAsync(
        string searchTerm,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllAsync(cancellationToken);

        var lowerSearchTerm = searchTerm.ToLower();
        return await _dbSet
            .Where(a => a.FirstName.ToLower().Contains(lowerSearchTerm) ||
                       a.LastName.ToLower().Contains(lowerSearchTerm))
            .OrderBy(a => a.LastName)
            .ThenBy(a => a.FirstName)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Author>> GetAuthorsByStateAsync(
        string state,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(state))
            return Enumerable.Empty<Author>();

        return await _dbSet
            .Where(a => a.State == state.ToUpper())
            .OrderBy(a => a.City)
            .ThenBy(a => a.LastName)
            .ThenBy(a => a.FirstName)
            .ToListAsync(cancellationToken);
    }
}

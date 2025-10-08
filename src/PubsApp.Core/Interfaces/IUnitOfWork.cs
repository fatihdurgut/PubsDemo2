namespace PubsApp.Core.Interfaces;

/// <summary>
/// Unit of Work pattern to manage transactions across multiple repositories
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Author repository
    /// </summary>
    IAuthorRepository Authors { get; }

    /// <summary>
    /// Title repository
    /// </summary>
    ITitleRepository Titles { get; }

    /// <summary>
    /// Publisher repository
    /// </summary>
    IPublisherRepository Publishers { get; }

    /// <summary>
    /// Saves all changes made in this unit of work
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins a database transaction
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits the current transaction
    /// </summary>
    Task CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back the current transaction
    /// </summary>
    Task RollbackAsync(CancellationToken cancellationToken = default);
}

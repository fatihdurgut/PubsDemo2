using Microsoft.EntityFrameworkCore.Storage;
using PubsApp.Core.Interfaces;
using PubsApp.Infrastructure.Data;

namespace PubsApp.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementation for managing transactions and repository coordination
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly PubsDbContext _context;
    private IDbContextTransaction? _transaction;
    private bool _disposed;

    // Lazy initialization of repositories
    private IAuthorRepository? _authorRepository;
    private ITitleRepository? _titleRepository;
    private IPublisherRepository? _publisherRepository;

    public UnitOfWork(PubsDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    public IAuthorRepository Authors => 
        _authorRepository ??= new AuthorRepository(_context);

    /// <inheritdoc />
    public ITitleRepository Titles => 
        _titleRepository ??= new TitleRepository(_context);

    /// <inheritdoc />
    public IPublisherRepository Publishers => 
        _publisherRepository ??= new PublisherRepository(_context);

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
            throw new InvalidOperationException("A transaction is already in progress.");

        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("No transaction is in progress.");

        try
        {
            await SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    /// <inheritdoc />
    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    /// <summary>
    /// Disposes the Unit of Work and releases resources
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the Unit of Work asynchronously
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        Dispose(false);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _context?.Dispose();
            }
            _disposed = true;
        }
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
        }

        if (_context != null)
        {
            await _context.DisposeAsync();
        }
    }
}

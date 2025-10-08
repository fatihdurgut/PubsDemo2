using MediatR;
using PubsApp.Core.Interfaces;

namespace PubsApp.Application.Features.Authors.Commands;

/// <summary>
/// Command to delete an author
/// </summary>
public record DeleteAuthorCommand(string AuthorId) : IRequest<bool>;

/// <summary>
/// Handler for DeleteAuthorCommand
/// </summary>
public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAuthorCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<bool> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
    {
        // Get existing author
        var author = await _unitOfWork.Authors.GetByIdAsync(request.AuthorId, cancellationToken);
        if (author == null)
        {
            throw new KeyNotFoundException($"Author with ID {request.AuthorId} not found");
        }

        // Delete from repository
        await _unitOfWork.Authors.DeleteAsync(author, cancellationToken);

        // Save changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}

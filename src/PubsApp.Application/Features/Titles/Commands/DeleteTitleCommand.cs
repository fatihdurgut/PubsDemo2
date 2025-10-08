using MediatR;
using PubsApp.Core.Interfaces;

namespace PubsApp.Application.Features.Titles.Commands;

/// <summary>
/// Command to delete a title
/// </summary>
public record DeleteTitleCommand(string TitleId) : IRequest<bool>;

/// <summary>
/// Handler for DeleteTitleCommand
/// </summary>
public class DeleteTitleCommandHandler : IRequestHandler<DeleteTitleCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTitleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<bool> Handle(DeleteTitleCommand request, CancellationToken cancellationToken)
    {
        // Get existing title
        var title = await _unitOfWork.Titles.GetByIdAsync(request.TitleId, cancellationToken);
        if (title == null)
        {
            throw new KeyNotFoundException($"Title with ID {request.TitleId} not found");
        }

        // Delete from repository
        await _unitOfWork.Titles.DeleteAsync(title, cancellationToken);

        // Save changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}

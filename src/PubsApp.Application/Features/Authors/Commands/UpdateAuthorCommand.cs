using AutoMapper;
using MediatR;
using PubsApp.Application.DTOs;
using PubsApp.Core.Interfaces;

namespace PubsApp.Application.Features.Authors.Commands;

/// <summary>
/// Command to update an existing author
/// </summary>
public record UpdateAuthorCommand(string AuthorId, UpdateAuthorDto Author) : IRequest<AuthorDto>;

/// <summary>
/// Handler for UpdateAuthorCommand
/// </summary>
public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, AuthorDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateAuthorCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<AuthorDto> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        // Get existing author
        var existingAuthor = await _unitOfWork.Authors.GetByIdAsync(request.AuthorId, cancellationToken);
        if (existingAuthor == null)
        {
            throw new KeyNotFoundException($"Author with ID {request.AuthorId} not found");
        }

        // Map DTO to entity (updates only changed fields)
        _mapper.Map(request.Author, existingAuthor);

        // Update in repository
        await _unitOfWork.Authors.UpdateAsync(existingAuthor, cancellationToken);

        // Save changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Map entity back to DTO and return
        return _mapper.Map<AuthorDto>(existingAuthor);
    }
}

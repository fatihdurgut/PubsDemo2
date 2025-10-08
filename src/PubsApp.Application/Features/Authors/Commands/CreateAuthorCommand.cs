using AutoMapper;
using MediatR;
using PubsApp.Application.DTOs;
using PubsApp.Core.Entities;
using PubsApp.Core.Interfaces;

namespace PubsApp.Application.Features.Authors.Commands;

/// <summary>
/// Command to create a new author
/// </summary>
public record CreateAuthorCommand(CreateAuthorDto Author) : IRequest<AuthorDto>;

/// <summary>
/// Handler for CreateAuthorCommand
/// </summary>
public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, AuthorDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateAuthorCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<AuthorDto> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        // Check if author with same ID already exists
        var existingAuthor = await _unitOfWork.Authors.GetByIdAsync(request.Author.AuthorId, cancellationToken);
        if (existingAuthor != null)
        {
            throw new InvalidOperationException($"Author with ID {request.Author.AuthorId} already exists");
        }

        // Map DTO to entity
        var author = _mapper.Map<Author>(request.Author);

        // Add to repository
        await _unitOfWork.Authors.AddAsync(author, cancellationToken);

        // Save changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Map entity back to DTO and return
        return _mapper.Map<AuthorDto>(author);
    }
}

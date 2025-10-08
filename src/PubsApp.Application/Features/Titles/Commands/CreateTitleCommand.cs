using AutoMapper;
using MediatR;
using PubsApp.Application.DTOs;
using PubsApp.Core.Entities;
using PubsApp.Core.Interfaces;

namespace PubsApp.Application.Features.Titles.Commands;

/// <summary>
/// Command to create a new title
/// </summary>
public record CreateTitleCommand(CreateTitleDto Title) : IRequest<TitleDto>;

/// <summary>
/// Handler for CreateTitleCommand
/// </summary>
public class CreateTitleCommandHandler : IRequestHandler<CreateTitleCommand, TitleDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTitleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<TitleDto> Handle(CreateTitleCommand request, CancellationToken cancellationToken)
    {
        // Check if title with same ID already exists
        var existingTitle = await _unitOfWork.Titles.GetByIdAsync(request.Title.TitleId, cancellationToken);
        if (existingTitle != null)
        {
            throw new InvalidOperationException($"Title with ID {request.Title.TitleId} already exists");
        }

        // Validate publisher exists if provided
        if (!string.IsNullOrWhiteSpace(request.Title.PublisherId))
        {
            var publisher = await _unitOfWork.Publishers.GetByIdAsync(request.Title.PublisherId, cancellationToken);
            if (publisher == null)
            {
                throw new InvalidOperationException($"Publisher with ID {request.Title.PublisherId} not found");
            }
        }

        // Map DTO to entity
        var title = _mapper.Map<Title>(request.Title);

        // Add to repository
        await _unitOfWork.Titles.AddAsync(title, cancellationToken);

        // Save changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Map entity back to DTO and return
        return _mapper.Map<TitleDto>(title);
    }
}

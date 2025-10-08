using AutoMapper;
using MediatR;
using PubsApp.Application.DTOs;
using PubsApp.Core.Interfaces;

namespace PubsApp.Application.Features.Titles.Commands;

/// <summary>
/// Command to update an existing title
/// </summary>
public record UpdateTitleCommand(string TitleId, UpdateTitleDto Title) : IRequest<TitleDto>;

/// <summary>
/// Handler for UpdateTitleCommand
/// </summary>
public class UpdateTitleCommandHandler : IRequestHandler<UpdateTitleCommand, TitleDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateTitleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<TitleDto> Handle(UpdateTitleCommand request, CancellationToken cancellationToken)
    {
        // Get existing title
        var existingTitle = await _unitOfWork.Titles.GetByIdAsync(request.TitleId, cancellationToken);
        if (existingTitle == null)
        {
            throw new KeyNotFoundException($"Title with ID {request.TitleId} not found");
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

        // Map DTO to entity (updates only changed fields)
        _mapper.Map(request.Title, existingTitle);

        // Update in repository
        await _unitOfWork.Titles.UpdateAsync(existingTitle, cancellationToken);

        // Save changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Map entity back to DTO and return
        return _mapper.Map<TitleDto>(existingTitle);
    }
}

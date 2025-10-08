using AutoMapper;
using MediatR;
using PubsApp.Application.Common;
using PubsApp.Application.DTOs;
using PubsApp.Core.Entities;
using PubsApp.Core.Interfaces;

namespace PubsApp.Application.Features.Publishers.Commands;

/// <summary>
/// Command to create a new publisher
/// </summary>
public record CreatePublisherCommand(CreatePublisherDto Publisher) : IRequest<PublisherDto>;

/// <summary>
/// Handler for CreatePublisherCommand
/// </summary>
public class CreatePublisherCommandHandler : IRequestHandler<CreatePublisherCommand, PublisherDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePublisherCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<PublisherDto> Handle(CreatePublisherCommand request, CancellationToken cancellationToken)
    {
        // Check if publisher with same ID already exists
        var existingPublisher = await _unitOfWork.Publishers.GetByIdAsync(request.Publisher.PublisherId, cancellationToken);
        if (existingPublisher != null)
        {
            throw new InvalidOperationException($"Publisher with ID {request.Publisher.PublisherId} already exists");
        }

        // Map DTO to entity
        var publisher = _mapper.Map<Publisher>(request.Publisher);

        // Add to repository
        await _unitOfWork.Publishers.AddAsync(publisher, cancellationToken);

        // Save changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Map entity back to DTO and return
        return _mapper.Map<PublisherDto>(publisher);
    }
}

/// <summary>
/// Command to update an existing publisher
/// </summary>
public record UpdatePublisherCommand(string PublisherId, UpdatePublisherDto Publisher) : IRequest<PublisherDto>;

/// <summary>
/// Handler for UpdatePublisherCommand
/// </summary>
public class UpdatePublisherCommandHandler : IRequestHandler<UpdatePublisherCommand, PublisherDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePublisherCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<PublisherDto> Handle(UpdatePublisherCommand request, CancellationToken cancellationToken)
    {
        // Get existing publisher
        var existingPublisher = await _unitOfWork.Publishers.GetByIdAsync(request.PublisherId, cancellationToken);
        if (existingPublisher == null)
        {
            throw new KeyNotFoundException($"Publisher with ID {request.PublisherId} not found");
        }

        // Map DTO to entity (updates only changed fields)
        _mapper.Map(request.Publisher, existingPublisher);

        // Update in repository
        await _unitOfWork.Publishers.UpdateAsync(existingPublisher, cancellationToken);

        // Save changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Map entity back to DTO and return
        return _mapper.Map<PublisherDto>(existingPublisher);
    }
}

/// <summary>
/// Command to delete a publisher
/// </summary>
public record DeletePublisherCommand(string PublisherId) : IRequest<bool>;

/// <summary>
/// Handler for DeletePublisherCommand
/// </summary>
public class DeletePublisherCommandHandler : IRequestHandler<DeletePublisherCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePublisherCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<bool> Handle(DeletePublisherCommand request, CancellationToken cancellationToken)
    {
        // Get existing publisher
        var publisher = await _unitOfWork.Publishers.GetByIdAsync(request.PublisherId, cancellationToken);
        if (publisher == null)
        {
            throw new KeyNotFoundException($"Publisher with ID {request.PublisherId} not found");
        }

        // Delete from repository
        await _unitOfWork.Publishers.DeleteAsync(publisher, cancellationToken);

        // Save changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}

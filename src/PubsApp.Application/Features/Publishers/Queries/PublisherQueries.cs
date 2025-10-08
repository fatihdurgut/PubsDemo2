using AutoMapper;
using MediatR;
using PubsApp.Application.Common;
using PubsApp.Application.DTOs;
using PubsApp.Core.Interfaces;

namespace PubsApp.Application.Features.Publishers.Queries;

/// <summary>
/// Query to get a publisher by ID
/// </summary>
public record GetPublisherByIdQuery(string PublisherId) : IRequest<PublisherDto?>;

/// <summary>
/// Handler for GetPublisherByIdQuery
/// </summary>
public class GetPublisherByIdQueryHandler : IRequestHandler<GetPublisherByIdQuery, PublisherDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPublisherByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<PublisherDto?> Handle(GetPublisherByIdQuery request, CancellationToken cancellationToken)
    {
        var publisher = await _unitOfWork.Publishers.GetByIdAsync(request.PublisherId, cancellationToken);
        return publisher != null ? _mapper.Map<PublisherDto>(publisher) : null;
    }
}

/// <summary>
/// Query to get all publishers with optional pagination
/// </summary>
public record GetAllPublishersQuery(PaginationParams? Pagination = null) : IRequest<PagedResult<PublisherDto>>;

/// <summary>
/// Handler for GetAllPublishersQuery
/// </summary>
public class GetAllPublishersQueryHandler : IRequestHandler<GetAllPublishersQuery, PagedResult<PublisherDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllPublishersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<PagedResult<PublisherDto>> Handle(GetAllPublishersQuery request, CancellationToken cancellationToken)
    {
        var pagination = request.Pagination ?? new PaginationParams();
        
        // Get all publishers (IQueryable is materialized in Infrastructure layer)
        var allPublishers = await _unitOfWork.Publishers.GetAllAsync(cancellationToken);
        var publishersList = allPublishers.OrderBy(p => p.PublisherName).ToList();
        
        // Get total count
        var totalCount = publishersList.Count;
        
        // Apply pagination manually
        var pagedPublishers = publishersList
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();
        
        // Map to DTOs
        var publisherDtos = _mapper.Map<IEnumerable<PublisherDto>>(pagedPublishers);
        
        return new PagedResult<PublisherDto>(publisherDtos, totalCount, pagination.PageNumber, pagination.PageSize);
    }
}

/// <summary>
/// Query to get publishers with their titles included
/// </summary>
public record GetPublishersWithTitlesQuery() : IRequest<IEnumerable<PublisherDto>>;

/// <summary>
/// Handler for GetPublishersWithTitlesQuery
/// </summary>
public class GetPublishersWithTitlesQueryHandler : IRequestHandler<GetPublishersWithTitlesQuery, IEnumerable<PublisherDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPublishersWithTitlesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<PublisherDto>> Handle(GetPublishersWithTitlesQuery request, CancellationToken cancellationToken)
    {
        var publishers = await _unitOfWork.Publishers.GetPublishersWithTitlesAsync(cancellationToken);
        return _mapper.Map<IEnumerable<PublisherDto>>(publishers);
    }
}

/// <summary>
/// Query to search publishers by name, city, state, or country
/// </summary>
public record SearchPublishersQuery(string SearchTerm) : IRequest<IEnumerable<PublisherDto>>;

/// <summary>
/// Handler for SearchPublishersQuery
/// </summary>
public class SearchPublishersQueryHandler : IRequestHandler<SearchPublishersQuery, IEnumerable<PublisherDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SearchPublishersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<PublisherDto>> Handle(SearchPublishersQuery request, CancellationToken cancellationToken)
    {
        var publishers = await _unitOfWork.Publishers.SearchByNameAsync(request.SearchTerm, cancellationToken);
        return _mapper.Map<IEnumerable<PublisherDto>>(publishers);
    }
}

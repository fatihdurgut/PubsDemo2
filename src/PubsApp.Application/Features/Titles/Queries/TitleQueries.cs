using AutoMapper;
using MediatR;
using PubsApp.Application.Common;
using PubsApp.Application.DTOs;
using PubsApp.Core.Interfaces;

namespace PubsApp.Application.Features.Titles.Queries;

/// <summary>
/// Query to get a title by ID
/// </summary>
public record GetTitleByIdQuery(string TitleId) : IRequest<TitleDto?>;

/// <summary>
/// Handler for GetTitleByIdQuery
/// </summary>
public class GetTitleByIdQueryHandler : IRequestHandler<GetTitleByIdQuery, TitleDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTitleByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<TitleDto?> Handle(GetTitleByIdQuery request, CancellationToken cancellationToken)
    {
        var title = await _unitOfWork.Titles.GetByIdAsync(request.TitleId, cancellationToken);
        return title == null ? null : _mapper.Map<TitleDto>(title);
    }
}

/// <summary>
/// Query to get all titles with optional pagination
/// </summary>
public record GetAllTitlesQuery(PaginationParams? Pagination = null) : IRequest<PagedResult<TitleDto>>;

/// <summary>
/// Handler for GetAllTitlesQuery
/// </summary>
public class GetAllTitlesQueryHandler : IRequestHandler<GetAllTitlesQuery, PagedResult<TitleDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllTitlesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<PagedResult<TitleDto>> Handle(GetAllTitlesQuery request, CancellationToken cancellationToken)
    {
        var titles = await _unitOfWork.Titles.GetAllAsync(cancellationToken);
        var titlesList = titles.ToList();

        // Apply pagination if provided
        if (request.Pagination != null)
        {
            var totalCount = titlesList.Count;
            var pagedTitles = titlesList
                .Skip(request.Pagination.Skip)
                .Take(request.Pagination.PageSize)
                .ToList();

            var dtos = _mapper.Map<IEnumerable<TitleDto>>(pagedTitles);
            return new PagedResult<TitleDto>(dtos, totalCount, request.Pagination.PageNumber, request.Pagination.PageSize);
        }

        // Return all without pagination
        var allDtos = _mapper.Map<IEnumerable<TitleDto>>(titlesList);
        return new PagedResult<TitleDto>(allDtos, titlesList.Count, 1, titlesList.Count);
    }
}

/// <summary>
/// Query to get titles with details (publisher and authors)
/// </summary>
public record GetTitlesWithDetailsQuery : IRequest<IEnumerable<TitleDto>>;

/// <summary>
/// Handler for GetTitlesWithDetailsQuery
/// </summary>
public class GetTitlesWithDetailsQueryHandler : IRequestHandler<GetTitlesWithDetailsQuery, IEnumerable<TitleDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTitlesWithDetailsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<TitleDto>> Handle(GetTitlesWithDetailsQuery request, CancellationToken cancellationToken)
    {
        var titles = await _unitOfWork.Titles.GetTitlesWithDetailsAsync(cancellationToken);
        return _mapper.Map<IEnumerable<TitleDto>>(titles);
    }
}

/// <summary>
/// Query to search titles
/// </summary>
public record SearchTitlesQuery(string SearchTerm) : IRequest<IEnumerable<TitleDto>>;

/// <summary>
/// Handler for SearchTitlesQuery
/// </summary>
public class SearchTitlesQueryHandler : IRequestHandler<SearchTitlesQuery, IEnumerable<TitleDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SearchTitlesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<TitleDto>> Handle(SearchTitlesQuery request, CancellationToken cancellationToken)
    {
        var titles = await _unitOfWork.Titles.SearchTitlesAsync(request.SearchTerm, cancellationToken);
        return _mapper.Map<IEnumerable<TitleDto>>(titles);
    }
}

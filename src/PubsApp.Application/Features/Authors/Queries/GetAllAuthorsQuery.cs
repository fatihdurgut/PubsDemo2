using AutoMapper;
using MediatR;
using PubsApp.Application.Common;
using PubsApp.Application.DTOs;
using PubsApp.Core.Interfaces;

namespace PubsApp.Application.Features.Authors.Queries;

/// <summary>
/// Query to get all authors with optional pagination
/// </summary>
public record GetAllAuthorsQuery(PaginationParams? Pagination = null) : IRequest<PagedResult<AuthorDto>>;

/// <summary>
/// Handler for GetAllAuthorsQuery
/// </summary>
public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, PagedResult<AuthorDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllAuthorsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<PagedResult<AuthorDto>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
    {
        var authors = await _unitOfWork.Authors.GetAllAsync(cancellationToken);
        var authorsList = authors.ToList();

        // Apply pagination if provided
        if (request.Pagination != null)
        {
            var totalCount = authorsList.Count;
            var pagedAuthors = authorsList
                .Skip(request.Pagination.Skip)
                .Take(request.Pagination.PageSize)
                .ToList();

            var dtos = _mapper.Map<IEnumerable<AuthorDto>>(pagedAuthors);
            return new PagedResult<AuthorDto>(dtos, totalCount, request.Pagination.PageNumber, request.Pagination.PageSize);
        }

        // Return all without pagination
        var allDtos = _mapper.Map<IEnumerable<AuthorDto>>(authorsList);
        return new PagedResult<AuthorDto>(allDtos, authorsList.Count, 1, authorsList.Count);
    }
}

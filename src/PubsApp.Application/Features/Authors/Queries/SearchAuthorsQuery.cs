using AutoMapper;
using MediatR;
using PubsApp.Application.DTOs;
using PubsApp.Core.Interfaces;

namespace PubsApp.Application.Features.Authors.Queries;

/// <summary>
/// Query to search authors by name
/// </summary>
public record SearchAuthorsQuery(string SearchTerm) : IRequest<IEnumerable<AuthorDto>>;

/// <summary>
/// Handler for SearchAuthorsQuery
/// </summary>
public class SearchAuthorsQueryHandler : IRequestHandler<SearchAuthorsQuery, IEnumerable<AuthorDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SearchAuthorsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<AuthorDto>> Handle(SearchAuthorsQuery request, CancellationToken cancellationToken)
    {
        var authors = await _unitOfWork.Authors.SearchByNameAsync(request.SearchTerm, cancellationToken);
        return _mapper.Map<IEnumerable<AuthorDto>>(authors);
    }
}

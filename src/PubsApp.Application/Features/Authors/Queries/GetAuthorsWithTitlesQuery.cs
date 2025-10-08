using AutoMapper;
using MediatR;
using PubsApp.Application.DTOs;
using PubsApp.Core.Interfaces;

namespace PubsApp.Application.Features.Authors.Queries;

/// <summary>
/// Query to get authors with their titles
/// </summary>
public record GetAuthorsWithTitlesQuery : IRequest<IEnumerable<AuthorDto>>;

/// <summary>
/// Handler for GetAuthorsWithTitlesQuery
/// </summary>
public class GetAuthorsWithTitlesQueryHandler : IRequestHandler<GetAuthorsWithTitlesQuery, IEnumerable<AuthorDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAuthorsWithTitlesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<AuthorDto>> Handle(GetAuthorsWithTitlesQuery request, CancellationToken cancellationToken)
    {
        var authors = await _unitOfWork.Authors.GetAuthorsWithTitlesAsync(cancellationToken);
        return _mapper.Map<IEnumerable<AuthorDto>>(authors);
    }
}

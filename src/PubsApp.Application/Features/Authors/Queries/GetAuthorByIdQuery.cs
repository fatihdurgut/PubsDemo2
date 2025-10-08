using AutoMapper;
using MediatR;
using PubsApp.Application.DTOs;
using PubsApp.Core.Interfaces;

namespace PubsApp.Application.Features.Authors.Queries;

/// <summary>
/// Query to get an author by ID
/// </summary>
public record GetAuthorByIdQuery(string AuthorId) : IRequest<AuthorDto?>;

/// <summary>
/// Handler for GetAuthorByIdQuery
/// </summary>
public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, AuthorDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAuthorByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<AuthorDto?> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        var author = await _unitOfWork.Authors.GetByIdAsync(request.AuthorId, cancellationToken);
        return author == null ? null : _mapper.Map<AuthorDto>(author);
    }
}

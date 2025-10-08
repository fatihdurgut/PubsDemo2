using MediatR;
using Microsoft.AspNetCore.Mvc;
using PubsApp.Application.Common;
using PubsApp.Application.DTOs;
using PubsApp.Application.Features.Authors.Commands;
using PubsApp.Application.Features.Authors.Queries;

namespace PubsApp.API.Controllers;

/// <summary>
/// API controller for managing authors
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthorsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthorsController> _logger;

    public AuthorsController(IMediator mediator, ILogger<AuthorsController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get all authors with optional pagination
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10, max: 100)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paged list of authors</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResult<AuthorDto>>> GetAuthors(
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Retrieving authors with pagination: Page {PageNumber}, Size {PageSize}", pageNumber, pageSize);
            
            var pagination = pageNumber.HasValue || pageSize.HasValue
                ? new PaginationParams { PageNumber = pageNumber ?? 1, PageSize = pageSize ?? 10 }
                : null;

            var query = new GetAllAuthorsQuery(pagination);
            var result = await _mediator.Send(query, cancellationToken);
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving authors");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving authors");
        }
    }

    /// <summary>
    /// Get authors with their titles
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of authors with their titles</returns>
    [HttpGet("with-titles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthorsWithTitles(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Retrieving all authors with titles");
            var query = new GetAuthorsWithTitlesQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving authors with titles");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving authors with titles");
        }
    }

    /// <summary>
    /// Get an author by ID
    /// </summary>
    /// <param name="id">Author ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Author details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AuthorDto>> GetAuthor(string id, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Retrieving author with ID: {AuthorId}", id);
            var query = new GetAuthorByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            
            if (result == null)
            {
                _logger.LogWarning("Author with ID {AuthorId} not found", id);
                return NotFound($"Author with ID {id} not found");
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving author with ID: {AuthorId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the author");
        }
    }

    /// <summary>
    /// Search authors by name
    /// </summary>
    /// <param name="searchTerm">Search term for author name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of matching authors</returns>
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> SearchAuthors(
        [FromQuery] string searchTerm,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Search term cannot be empty");
            }

            _logger.LogInformation("Searching authors with term: {SearchTerm}", searchTerm);
            var query = new SearchAuthorsQuery(searchTerm);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching authors with term: {SearchTerm}", searchTerm);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while searching authors");
        }
    }

    /// <summary>
    /// Create a new author
    /// </summary>
    /// <param name="createAuthorDto">Author data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created author</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AuthorDto>> CreateAuthor(
        [FromBody] CreateAuthorDto createAuthorDto,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Creating new author with ID: {AuthorId}", createAuthorDto.AuthorId);
            var command = new CreateAuthorCommand(createAuthorDto);
            var result = await _mediator.Send(command, cancellationToken);
            
            return CreatedAtAction(nameof(GetAuthor), new { id = result.AuthorId }, result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to create author: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating author");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the author");
        }
    }

    /// <summary>
    /// Update an existing author
    /// </summary>
    /// <param name="id">Author ID</param>
    /// <param name="updateAuthorDto">Updated author data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated author</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AuthorDto>> UpdateAuthor(
        string id,
        [FromBody] UpdateAuthorDto updateAuthorDto,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Updating author with ID: {AuthorId}", id);
            var command = new UpdateAuthorCommand(id, updateAuthorDto);
            var result = await _mediator.Send(command, cancellationToken);
            
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Author not found: {Message}", ex.Message);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating author with ID: {AuthorId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the author");
        }
    }

    /// <summary>
    /// Delete an author
    /// </summary>
    /// <param name="id">Author ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAuthor(string id, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Deleting author with ID: {AuthorId}", id);
            var command = new DeleteAuthorCommand(id);
            await _mediator.Send(command, cancellationToken);
            
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Author not found: {Message}", ex.Message);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting author with ID: {AuthorId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the author");
        }
    }
}

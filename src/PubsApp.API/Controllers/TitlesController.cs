using MediatR;
using Microsoft.AspNetCore.Mvc;
using PubsApp.Application.Common;
using PubsApp.Application.DTOs;
using PubsApp.Application.Features.Titles.Commands;
using PubsApp.Application.Features.Titles.Queries;

namespace PubsApp.API.Controllers;

/// <summary>
/// Controller for managing titles
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TitlesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TitlesController> _logger;

    public TitlesController(IMediator mediator, ILogger<TitlesController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get all titles with optional pagination
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10, max: 100)</param>
    /// <returns>Paginated list of titles</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<TitleDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<TitleDto>>> GetAllTitles([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var query = new GetAllTitlesQuery(new PaginationParams { PageNumber = pageNumber, PageSize = pageSize });
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all titles");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving titles");
        }
    }

    /// <summary>
    /// Get a title by ID
    /// </summary>
    /// <param name="id">Title ID</param>
    /// <returns>Title details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TitleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TitleDto>> GetTitleById(string id)
    {
        try
        {
            var query = new GetTitleByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound($"Title with ID {id} not found");
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting title {TitleId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the title");
        }
    }

    /// <summary>
    /// Get titles with detailed information (including publisher and authors)
    /// </summary>
    /// <returns>List of titles with details</returns>
    [HttpGet("with-details")]
    [ProducesResponseType(typeof(IEnumerable<TitleDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TitleDto>>> GetTitlesWithDetails()
    {
        try
        {
            var query = new GetTitlesWithDetailsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting titles with details");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving titles with details");
        }
    }

    /// <summary>
    /// Search titles by name, type, or notes
    /// </summary>
    /// <param name="searchTerm">Search term</param>
    /// <returns>List of matching titles</returns>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<TitleDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TitleDto>>> SearchTitles([FromQuery] string searchTerm)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Search term cannot be empty");
            }

            var query = new SearchTitlesQuery(searchTerm);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching titles with term {SearchTerm}", searchTerm);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while searching titles");
        }
    }

    /// <summary>
    /// Create a new title
    /// </summary>
    /// <param name="createTitleDto">Title data</param>
    /// <returns>Created title</returns>
    [HttpPost]
    [ProducesResponseType(typeof(TitleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TitleDto>> CreateTitle([FromBody] CreateTitleDto createTitleDto)
    {
        try
        {
            var command = new CreateTitleCommand(createTitleDto);
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetTitleById), new { id = result.TitleId }, result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation while creating title");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating title");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the title");
        }
    }

    /// <summary>
    /// Update an existing title
    /// </summary>
    /// <param name="id">Title ID</param>
    /// <param name="updateTitleDto">Updated title data</param>
    /// <returns>Updated title</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TitleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TitleDto>> UpdateTitle(string id, [FromBody] UpdateTitleDto updateTitleDto)
    {
        try
        {
            var command = new UpdateTitleCommand(id, updateTitleDto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Title {TitleId} not found for update", id);
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation while updating title {TitleId}", id);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating title {TitleId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the title");
        }
    }

    /// <summary>
    /// Delete a title
    /// </summary>
    /// <param name="id">Title ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTitle(string id)
    {
        try
        {
            var command = new DeleteTitleCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Title {TitleId} not found for deletion", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting title {TitleId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the title");
        }
    }
}

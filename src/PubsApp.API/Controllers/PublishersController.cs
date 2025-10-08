using MediatR;
using Microsoft.AspNetCore.Mvc;
using PubsApp.Application.Common;
using PubsApp.Application.DTOs;
using PubsApp.Application.Features.Publishers.Commands;
using PubsApp.Application.Features.Publishers.Queries;

namespace PubsApp.API.Controllers;

/// <summary>
/// Controller for managing publishers
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PublishersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PublishersController> _logger;

    public PublishersController(IMediator mediator, ILogger<PublishersController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get all publishers with optional pagination
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10, max: 100)</param>
    /// <returns>Paginated list of publishers</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<PublisherDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<PublisherDto>>> GetAllPublishers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var query = new GetAllPublishersQuery(new PaginationParams { PageNumber = pageNumber, PageSize = pageSize });
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all publishers");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving publishers");
        }
    }

    /// <summary>
    /// Get a publisher by ID
    /// </summary>
    /// <param name="id">Publisher ID</param>
    /// <returns>Publisher details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PublisherDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PublisherDto>> GetPublisherById(string id)
    {
        try
        {
            var query = new GetPublisherByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound($"Publisher with ID {id} not found");
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting publisher {PublisherId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the publisher");
        }
    }

    /// <summary>
    /// Get publishers with their titles included
    /// </summary>
    /// <returns>List of publishers with titles</returns>
    [HttpGet("with-titles")]
    [ProducesResponseType(typeof(IEnumerable<PublisherDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PublisherDto>>> GetPublishersWithTitles()
    {
        try
        {
            var query = new GetPublishersWithTitlesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting publishers with titles");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving publishers with titles");
        }
    }

    /// <summary>
    /// Search publishers by name, city, state, or country
    /// </summary>
    /// <param name="searchTerm">Search term</param>
    /// <returns>List of matching publishers</returns>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<PublisherDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PublisherDto>>> SearchPublishers([FromQuery] string searchTerm)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Search term cannot be empty");
            }

            var query = new SearchPublishersQuery(searchTerm);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching publishers with term {SearchTerm}", searchTerm);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while searching publishers");
        }
    }

    /// <summary>
    /// Create a new publisher
    /// </summary>
    /// <param name="createPublisherDto">Publisher data</param>
    /// <returns>Created publisher</returns>
    [HttpPost]
    [ProducesResponseType(typeof(PublisherDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PublisherDto>> CreatePublisher([FromBody] CreatePublisherDto createPublisherDto)
    {
        try
        {
            var command = new CreatePublisherCommand(createPublisherDto);
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPublisherById), new { id = result.PublisherId }, result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation while creating publisher");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating publisher");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the publisher");
        }
    }

    /// <summary>
    /// Update an existing publisher
    /// </summary>
    /// <param name="id">Publisher ID</param>
    /// <param name="updatePublisherDto">Updated publisher data</param>
    /// <returns>Updated publisher</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PublisherDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PublisherDto>> UpdatePublisher(string id, [FromBody] UpdatePublisherDto updatePublisherDto)
    {
        try
        {
            var command = new UpdatePublisherCommand(id, updatePublisherDto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Publisher {PublisherId} not found for update", id);
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation while updating publisher {PublisherId}", id);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating publisher {PublisherId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the publisher");
        }
    }

    /// <summary>
    /// Delete a publisher
    /// </summary>
    /// <param name="id">Publisher ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePublisher(string id)
    {
        try
        {
            var command = new DeletePublisherCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Publisher {PublisherId} not found for deletion", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting publisher {PublisherId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the publisher");
        }
    }
}

namespace PubsApp.Application.DTOs;

/// <summary>
/// DTO for creating a new publisher
/// </summary>
public class CreatePublisherDto
{
    /// <summary>
    /// Publisher identifier (required, 4 characters)
    /// </summary>
    public string PublisherId { get; set; } = string.Empty;

    /// <summary>
    /// Publisher name (optional, max 40 characters)
    /// </summary>
    public string? PublisherName { get; set; }

    /// <summary>
    /// City (optional, max 20 characters)
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// State code (optional, 2 letters)
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// Country (optional, max 30 characters)
    /// </summary>
    public string? Country { get; set; }
}

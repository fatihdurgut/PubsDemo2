namespace PubsApp.Application.DTOs;

/// <summary>
/// Summary DTO for Publisher (used in nested objects)
/// </summary>
public class PublisherSummaryDto
{
    /// <summary>
    /// Publisher identifier
    /// </summary>
    public string PublisherId { get; set; } = string.Empty;

    /// <summary>
    /// Publisher name
    /// </summary>
    public string? PublisherName { get; set; }

    /// <summary>
    /// City and state
    /// </summary>
    public string? Location { get; set; }
}

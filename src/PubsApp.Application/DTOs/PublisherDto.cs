namespace PubsApp.Application.DTOs;

/// <summary>
/// Data Transfer Object for Publisher entity
/// </summary>
public class PublisherDto
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
    /// City
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// State code (2 letters)
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// Country
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// List of titles published by this publisher
    /// </summary>
    public List<TitleSummaryDto> Titles { get; set; } = new();
}

namespace PubsApp.Application.DTOs;

/// <summary>
/// Summary DTO for Title (used in nested objects)
/// </summary>
public class TitleSummaryDto
{
    /// <summary>
    /// Title identifier
    /// </summary>
    public string TitleId { get; set; } = string.Empty;

    /// <summary>
    /// Title name
    /// </summary>
    public string TitleName { get; set; } = string.Empty;

    /// <summary>
    /// Book type
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Price of the book
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// Publication date
    /// </summary>
    public DateTime PublishedDate { get; set; }
}

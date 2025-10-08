namespace PubsApp.Application.DTOs;

/// <summary>
/// DTO for updating an existing title
/// </summary>
public class UpdateTitleDto
{
    /// <summary>
    /// Title name (required, max 80 characters)
    /// </summary>
    public string TitleName { get; set; } = string.Empty;

    /// <summary>
    /// Book type (required, max 12 characters)
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Publisher ID (optional, 4 characters)
    /// </summary>
    public string? PublisherId { get; set; }

    /// <summary>
    /// Price of the book (optional)
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// Advance paid to authors (optional)
    /// </summary>
    public decimal? Advance { get; set; }

    /// <summary>
    /// Royalty percentage (optional)
    /// </summary>
    public int? Royalty { get; set; }

    /// <summary>
    /// Year-to-date sales (optional)
    /// </summary>
    public int? YtdSales { get; set; }

    /// <summary>
    /// Notes about the book (optional, max 200 characters)
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Publication date (required)
    /// </summary>
    public DateTime PublishedDate { get; set; }
}

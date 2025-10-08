namespace PubsApp.Application.DTOs;

/// <summary>
/// Data Transfer Object for Title entity
/// </summary>
public class TitleDto
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
    /// Book type (e.g., business, mod_cook, psychology)
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Publisher ID
    /// </summary>
    public string? PublisherId { get; set; }

    /// <summary>
    /// Publisher information
    /// </summary>
    public PublisherSummaryDto? Publisher { get; set; }

    /// <summary>
    /// Price of the book
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// Advance paid to authors
    /// </summary>
    public decimal? Advance { get; set; }

    /// <summary>
    /// Royalty percentage
    /// </summary>
    public int? Royalty { get; set; }

    /// <summary>
    /// Year-to-date sales
    /// </summary>
    public int? YtdSales { get; set; }

    /// <summary>
    /// Notes about the book
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Publication date
    /// </summary>
    public DateTime PublishedDate { get; set; }

    /// <summary>
    /// List of authors for this title
    /// </summary>
    public List<AuthorSummaryDto> Authors { get; set; } = new();
}

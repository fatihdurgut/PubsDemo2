namespace PubsApp.Core.Entities;

/// <summary>
/// Represents a book title in the Pubs database
/// </summary>
public class Title
{
    /// <summary>
    /// Title identifier (title_id in database)
    /// </summary>
    public string TitleId { get; set; } = string.Empty;

    /// <summary>
    /// Book title
    /// </summary>
    public string TitleName { get; set; } = string.Empty;

    /// <summary>
    /// Type of book (business, psychology, etc.)
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Publisher identifier
    /// </summary>
    public string? PublisherId { get; set; }

    /// <summary>
    /// Book price
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// Advance payment to author
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
    /// Navigation property to publisher
    /// </summary>
    public Publisher? Publisher { get; set; }

    /// <summary>
    /// Navigation property for authors of this title
    /// </summary>
    public ICollection<TitleAuthor> TitleAuthors { get; set; } = new List<TitleAuthor>();
}

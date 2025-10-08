namespace PubsApp.Core.Entities;

/// <summary>
/// Represents the many-to-many relationship between titles and authors
/// </summary>
public class TitleAuthor
{
    /// <summary>
    /// Author identifier
    /// </summary>
    public string AuthorId { get; set; } = string.Empty;

    /// <summary>
    /// Title identifier
    /// </summary>
    public string TitleId { get; set; } = string.Empty;

    /// <summary>
    /// Author's order for this title
    /// </summary>
    public byte? AuthorOrder { get; set; }

    /// <summary>
    /// Royalty percentage for this author on this title
    /// </summary>
    public int? RoyaltyPercentage { get; set; }

    /// <summary>
    /// Navigation property to author
    /// </summary>
    public Author Author { get; set; } = null!;

    /// <summary>
    /// Navigation property to title
    /// </summary>
    public Title Title { get; set; } = null!;
}

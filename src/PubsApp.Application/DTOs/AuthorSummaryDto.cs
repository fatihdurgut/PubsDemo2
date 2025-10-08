namespace PubsApp.Application.DTOs;

/// <summary>
/// Summary DTO for Author (used in nested objects)
/// </summary>
public class AuthorSummaryDto
{
    /// <summary>
    /// Author identifier
    /// </summary>
    public string AuthorId { get; set; } = string.Empty;

    /// <summary>
    /// Author's full name
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Author order in the title
    /// </summary>
    public byte? AuthorOrder { get; set; }

    /// <summary>
    /// Royalty percentage for this title
    /// </summary>
    public int? RoyaltyPercentage { get; set; }
}

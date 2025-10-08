namespace PubsApp.Application.DTOs;

/// <summary>
/// Data Transfer Object for Author entity
/// </summary>
public class AuthorDto
{
    /// <summary>
    /// Author identifier
    /// </summary>
    public string AuthorId { get; set; } = string.Empty;

    /// <summary>
    /// Author's last name
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Author's first name
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Full name (computed property)
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>
    /// Phone number
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Street address
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// City
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// State code (2 letters)
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// ZIP code
    /// </summary>
    public string? Zip { get; set; }

    /// <summary>
    /// Contract status
    /// </summary>
    public bool Contract { get; set; }

    /// <summary>
    /// List of titles written by this author
    /// </summary>
    public List<TitleSummaryDto> Titles { get; set; } = new();
}

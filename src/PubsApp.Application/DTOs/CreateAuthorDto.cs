namespace PubsApp.Application.DTOs;

/// <summary>
/// DTO for creating a new author
/// </summary>
public class CreateAuthorDto
{
    /// <summary>
    /// Author identifier (must be unique, 11 characters max)
    /// </summary>
    public string AuthorId { get; set; } = string.Empty;

    /// <summary>
    /// Author's last name (required, max 40 characters)
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Author's first name (required, max 20 characters)
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Phone number (required, format: ###-###-####)
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Street address (optional, max 40 characters)
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// City (optional, max 20 characters)
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// State code (optional, 2 letters)
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// ZIP code (optional, 5 digits)
    /// </summary>
    public string? Zip { get; set; }

    /// <summary>
    /// Contract status (required)
    /// </summary>
    public bool Contract { get; set; }
}

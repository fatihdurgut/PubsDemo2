namespace PubsApp.Application.DTOs;

/// <summary>
/// DTO for updating an existing publisher
/// </summary>
public class UpdatePublisherDto
{
    /// <summary>
    /// Publisher name (optional, max 40 characters)
    /// </summary>
    public string? PublisherName { get; set; }

    /// <summary>
    /// City (optional, max 20 characters)
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// State code (optional, 2 letters)
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// Country (optional, max 30 characters)
    /// </summary>
    public string? Country { get; set; }
}

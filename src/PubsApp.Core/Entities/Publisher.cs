namespace PubsApp.Core.Entities;

/// <summary>
/// Represents a publisher in the Pubs database
/// </summary>
public class Publisher
{
    /// <summary>
    /// Publisher identifier (pub_id in database)
    /// </summary>
    public string PublisherId { get; set; } = string.Empty;

    /// <summary>
    /// Publisher's name
    /// </summary>
    public string? PublisherName { get; set; }

    /// <summary>
    /// Publisher's city
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// Publisher's state (2-character code)
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// Publisher's country
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// Navigation property for titles published by this publisher
    /// </summary>
    public ICollection<Title> Titles { get; set; } = new List<Title>();
}

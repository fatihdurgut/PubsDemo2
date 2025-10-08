namespace PubsApp.Core.Entities;

/// <summary>
/// Represents an author in the Pubs database
/// </summary>
public class Author
{
    /// <summary>
    /// Author identifier (au_id in database)
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
    /// Author's phone number
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Author's street address
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Author's city
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// Author's state (2-character code)
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// Author's ZIP code
    /// </summary>
    public string? Zip { get; set; }

    /// <summary>
    /// Indicates whether the author has a contract
    /// </summary>
    public bool Contract { get; set; }

    /// <summary>
    /// Navigation property for titles written by this author
    /// </summary>
    public ICollection<TitleAuthor> TitleAuthors { get; set; } = new List<TitleAuthor>();
}

namespace PubsApp.Application.Common;

/// <summary>
/// Parameters for pagination requests
/// </summary>
public class PaginationParams
{
    private const int MaxPageSize = 100;
    private int _pageSize = 10;

    /// <summary>
    /// Page number (1-based, default: 1)
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Number of items per page (default: 10, max: 100)
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    /// <summary>
    /// Number of items to skip
    /// </summary>
    public int Skip => (PageNumber - 1) * PageSize;
}

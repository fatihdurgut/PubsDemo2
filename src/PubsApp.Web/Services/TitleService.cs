using PubsApp.Application.DTOs;
using PubsApp.Application.Common;

namespace PubsApp.Web.Services;

/// <summary>
/// Service for Title API operations
/// </summary>
public class TitleService : ApiServiceBase
{
    private const string BaseEndpoint = "api/titles";

    public TitleService(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<PagedResult<TitleDto>?> GetAllAsync(int pageNumber = 1, int pageSize = 10)
    {
        return await GetAsync<PagedResult<TitleDto>>($"{BaseEndpoint}?pageNumber={pageNumber}&pageSize={pageSize}");
    }

    public async Task<TitleDto?> GetByIdAsync(string id)
    {
        return await GetAsync<TitleDto>($"{BaseEndpoint}/{Uri.EscapeDataString(id)}");
    }

    public async Task<List<TitleDto>?> SearchAsync(string searchTerm)
    {
        return await GetAsync<List<TitleDto>>($"{BaseEndpoint}/search?searchTerm={Uri.EscapeDataString(searchTerm)}");
    }

    public async Task<TitleDto?> CreateAsync(CreateTitleDto title)
    {
        return await PostAsync<TitleDto>(BaseEndpoint, title);
    }

    public async Task<TitleDto?> UpdateAsync(string id, UpdateTitleDto title)
    {
        return await PutAsync<TitleDto>($"{BaseEndpoint}/{Uri.EscapeDataString(id)}", title);
    }

    public new async Task DeleteAsync(string id)
    {
        await base.DeleteAsync($"{BaseEndpoint}/{Uri.EscapeDataString(id)}");
    }
}

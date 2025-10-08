using PubsApp.Application.DTOs;
using PubsApp.Application.Common;

namespace PubsApp.Web.Services;

/// <summary>
/// Service for Publisher API operations
/// </summary>
public class PublisherService : ApiServiceBase
{
    private const string BaseEndpoint = "api/publishers";

    public PublisherService(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<PagedResult<PublisherDto>?> GetAllAsync(int pageNumber = 1, int pageSize = 10)
    {
        return await GetAsync<PagedResult<PublisherDto>>($"{BaseEndpoint}?pageNumber={pageNumber}&pageSize={pageSize}");
    }

    public async Task<PublisherDto?> GetByIdAsync(string id)
    {
        return await GetAsync<PublisherDto>($"{BaseEndpoint}/{Uri.EscapeDataString(id)}");
    }

    public async Task<List<PublisherDto>?> SearchAsync(string searchTerm)
    {
        return await GetAsync<List<PublisherDto>>($"{BaseEndpoint}/search?searchTerm={Uri.EscapeDataString(searchTerm)}");
    }

    public async Task<PublisherDto?> CreateAsync(CreatePublisherDto publisher)
    {
        return await PostAsync<PublisherDto>(BaseEndpoint, publisher);
    }

    public async Task<PublisherDto?> UpdateAsync(string id, UpdatePublisherDto publisher)
    {
        return await PutAsync<PublisherDto>($"{BaseEndpoint}/{Uri.EscapeDataString(id)}", publisher);
    }

    public new async Task DeleteAsync(string id)
    {
        await base.DeleteAsync($"{BaseEndpoint}/{Uri.EscapeDataString(id)}");
    }
}

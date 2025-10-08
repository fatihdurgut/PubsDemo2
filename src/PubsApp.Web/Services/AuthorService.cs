using PubsApp.Application.DTOs;
using PubsApp.Application.Common;

namespace PubsApp.Web.Services;

/// <summary>
/// Service for Author API operations
/// </summary>
public class AuthorService : ApiServiceBase
{
    private const string BaseEndpoint = "api/authors";

    public AuthorService(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<PagedResult<AuthorDto>?> GetAllAsync(int pageNumber = 1, int pageSize = 10)
    {
        return await GetAsync<PagedResult<AuthorDto>>($"{BaseEndpoint}?pageNumber={pageNumber}&pageSize={pageSize}");
    }

    public async Task<AuthorDto?> GetByIdAsync(string id)
    {
        return await GetAsync<AuthorDto>($"{BaseEndpoint}/{Uri.EscapeDataString(id)}");
    }

    public async Task<List<AuthorDto>?> SearchAsync(string searchTerm)
    {
        return await GetAsync<List<AuthorDto>>($"{BaseEndpoint}/search?searchTerm={Uri.EscapeDataString(searchTerm)}");
    }

    public async Task<AuthorDto?> CreateAsync(CreateAuthorDto author)
    {
        return await PostAsync<AuthorDto>(BaseEndpoint, author);
    }

    public async Task<AuthorDto?> UpdateAsync(string id, UpdateAuthorDto author)
    {
        return await PutAsync<AuthorDto>($"{BaseEndpoint}/{Uri.EscapeDataString(id)}", author);
    }

    public new async Task DeleteAsync(string id)
    {
        await base.DeleteAsync($"{BaseEndpoint}/{Uri.EscapeDataString(id)}");
    }
}

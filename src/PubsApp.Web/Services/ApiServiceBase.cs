using System.Net.Http.Json;
using System.Text.Json;

namespace PubsApp.Web.Services;

/// <summary>
/// Base class for API services providing common HTTP operations
/// </summary>
public abstract class ApiServiceBase
{
    protected readonly HttpClient HttpClient;
    protected readonly JsonSerializerOptions JsonOptions;

    protected ApiServiceBase(HttpClient httpClient)
    {
        HttpClient = httpClient;
        JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    protected async Task<T?> GetAsync<T>(string endpoint)
    {
        try
        {
            var response = await HttpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>(JsonOptions);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP Error: {ex.Message}");
            throw;
        }
    }

    protected async Task<T?> PostAsync<T>(string endpoint, object data)
    {
        try
        {
            var response = await HttpClient.PostAsJsonAsync(endpoint, data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>(JsonOptions);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP Error: {ex.Message}");
            throw;
        }
    }

    protected async Task<T?> PutAsync<T>(string endpoint, object data)
    {
        try
        {
            var response = await HttpClient.PutAsJsonAsync(endpoint, data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>(JsonOptions);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP Error: {ex.Message}");
            throw;
        }
    }

    protected async Task<bool> DeleteAsync(string endpoint)
    {
        try
        {
            var response = await HttpClient.DeleteAsync(endpoint);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP Error: {ex.Message}");
            return false;
        }
    }
}

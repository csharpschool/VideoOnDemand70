using System.Text;
using VOD.Common.DTOs;

namespace VOD.Common.Services;

public class AdminService : IAdminService
{
    readonly MemebershipHttpClient _http;

    public AdminService(MemebershipHttpClient httpClient)
    {
        _http = httpClient;
    }

    public async Task<List<TDto>> GetAsync<TDto>()
    {
        try
        {
            using HttpResponseMessage response = await _http.Client.GetAsync($"courses?freeOnly=false");
            response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<List<TDto>>(await response.Content.ReadAsStreamAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result ?? new List<TDto>();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<TDto?> GetAsync<TDto>(int id)
    {
        try
        {
            using HttpResponseMessage response = await _http.Client.GetAsync($"courses/{id}");
            response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<TDto>(await response.Content.ReadAsStreamAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result ?? default;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task CreateAsync<TDto>(TDto dto)
    {
        try
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(dto),
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response = await _http.Client.PostAsync("courses", jsonContent);

            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task EditAsync<TDto>(TDto dto, int id)
    {
        try
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(dto),
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response = await _http.Client.PutAsync($"courses/{id}", jsonContent);

            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task DeleteAsync<TDto>(int id)
    {
        try
        {
            using HttpResponseMessage response = await _http.Client.DeleteAsync($"courses/{id}");

            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}

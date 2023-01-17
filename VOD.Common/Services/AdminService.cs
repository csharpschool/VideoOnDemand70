using System.Text;
using VOD.Common.DTOs;

namespace VOD.Common.Services;

public class AdminService : IAdminService
{
    readonly MembershipHttpClient _http;

    public AdminService(MembershipHttpClient httpClient)
    {
        _http = httpClient;
    }

    public async Task<List<TDto>> GetAsync<TDto>(string uri)
    {
        try
        {
            using HttpResponseMessage response = await _http.Client.GetAsync(uri);// $"courses?freeOnly=false");
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

    public async Task<TDto?> SingleAsync<TDto>(string uri)
    {
        try
        {
            using HttpResponseMessage response = await _http.Client.GetAsync(uri);//$"courses/{id}");
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

    public async Task CreateAsync<TDto>(string uri, TDto dto)
    {
        try
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(dto),
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response = await _http.Client.PostAsync(uri, jsonContent); //"courses", jsonContent);

            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task EditAsync<TDto>(string uri, TDto dto)
    {
        try
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(dto),
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response = await _http.Client.PutAsync(uri, jsonContent); //$"courses/{id}", jsonContent);

            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task DeleteAsync<TDto>(string uri)
    {
        try
        {
            using HttpResponseMessage response = await _http.Client.DeleteAsync(uri); //$"courses/{id}");

            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}

using VOD.Common.DTOs;

namespace VOD.Common.Services;

public class AdminService : IAdminService
{
    readonly MemebershipHttpClient _http;

    public AdminService(MemebershipHttpClient httpClient)
    {
        _http = httpClient;
    }

    public async Task<List<TDto>> Get<TDto>()
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

}

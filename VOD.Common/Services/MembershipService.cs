using Blazored.LocalStorage;

namespace VOD.Common.Services;

public class MembershipService : IMembershipService
{
    protected readonly MembershipHttpClient _http;
    private readonly IStorageService _storage;
    protected readonly ILocalStorageService _localStorage;

    public MembershipService(MembershipHttpClient httpClient, IStorageService storage, ILocalStorageService localStorage)
    {
        _http = httpClient;
        _storage = storage;
        _localStorage = localStorage;
    }

    public async Task<List<CourseDTO>> GetCoursesAsync()
    {
        try
        {
            var token = await _storage.GetAsync(AuthConstants.TokenName);

            bool freeOnly = JwtParser.ParseIsNotInRoleFromPayload(token, UserRole.Customer);

            _http.AddBearerToken(token);
            
            using HttpResponseMessage response = await _http.Client.GetAsync($"courses?freeOnly={freeOnly}");
            response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<List<CourseDTO>>(await response.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result ?? new List<CourseDTO>();
        }
        catch
        {
            return new List<CourseDTO>();
        }
    }

    public async Task<CourseDTO> GetCourseAsync(int? id)
    {
        try
        {
            if (id is null) return new CourseDTO();
            using HttpResponseMessage response = await _http.Client.GetAsync($"courses/{id}");
            response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<CourseDTO>(await response.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result ?? new CourseDTO();
        }
        catch
        {
            return new CourseDTO();
        }
    }

    public async Task<VideoDTO> GetVideoAsync(int? id)
    {
        try
        {
            if (id is null) return new VideoDTO();
            using HttpResponseMessage response = await _http.Client.GetAsync($"videos/{id}");
            response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<VideoDTO>(await response.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result ?? new VideoDTO();
        }
        catch
        {
            return new VideoDTO();
        }
    }
}

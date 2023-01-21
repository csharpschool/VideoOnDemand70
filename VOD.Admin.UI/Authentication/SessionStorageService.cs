namespace VOD.Admin.UI.Authentication;

public class SessionStorageService : IStorageService
{
    private readonly ProtectedSessionStorage _sessionStorage;

    public SessionStorageService(ProtectedSessionStorage sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    public async Task SetAsync(string key, string value) => await _sessionStorage.SetAsync(key, value);
    public async Task<string> GetAsync(string key)
    {
        try
        {
            var result = await _sessionStorage.GetAsync<string>(key);

            if (result.Success)
                return result.Value ?? string.Empty;
        }
        catch
        {
        }

        return string.Empty;
    }
    public async Task RemoveAsync(string key) => await _sessionStorage.DeleteAsync(key);
}

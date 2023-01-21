using Blazored.LocalStorage;

namespace VOD.Common.Services;

public class LocalStorageService : IStorageService
{
    private readonly ILocalStorageService _localStorage;
    public LocalStorageService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }
    
    public async Task<string> GetAsync(string key)
    {
        return await _localStorage.GetItemAsync<string>(key);
    }

    public async Task RemoveAsync(string key)
    {
        await _localStorage.RemoveItemAsync(key);
    }

    public async Task SetAsync(string key, string value)
    {
        await _localStorage.SetItemAsync(key, value);
    }
}

using System.Net;
using System.Text;
using VOD.Common.Models;

namespace VOD.Common.Services;

public class AuthenticationService : AuthenticationStateProvider
{
    private readonly AuthenticationHttpClient _http;
    private readonly IStorageService _storage;
    private readonly AuthenticationState _anonymous;

    public AuthenticationService(AuthenticationHttpClient httpClient, IStorageService storage)
    {
        _http = httpClient;
        _storage = storage;
        _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _storage.GetAsync(AuthConstants.TokenName);

        if (string.IsNullOrWhiteSpace(token)) return _anonymous;

        _http.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromPayload(token), AuthConstants.AuthenticationType)));
    }

    public async Task<AuthenticatedUserDTO?> Login(AuthenticationUserModel userForAuthentication)
    {
        try
        {
            var user = new LoginUserDTO(userForAuthentication.Email, userForAuthentication.Password);

            using StringContent jsonContent = new(
                JsonSerializer.Serialize(user),
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response = await _http.Client.PostAsync("tokens", jsonContent);

            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode.Equals(HttpStatusCode.Unauthorized) || string.IsNullOrWhiteSpace(responseContent))
            {
                var updateTokenUser = new TokenUserDTO(userForAuthentication.Email);

                using StringContent jsonUpdateTokenUser = new(
                    JsonSerializer.Serialize(updateTokenUser),
                    Encoding.UTF8,
                    "application/json");

                using HttpResponseMessage createResponse = await _http.Client.PostAsync("tokens/create", jsonUpdateTokenUser);

                createResponse.EnsureSuccessStatusCode();

                using HttpResponseMessage fetchResponse = await _http.Client.PostAsync("tokens", jsonContent);
                fetchResponse.EnsureSuccessStatusCode();
                responseContent = await fetchResponse.Content.ReadAsStringAsync();
            }

            var result = JsonSerializer.Deserialize<AuthenticatedUserDTO>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result is null) return default;

            await _storage.SetAsync(AuthConstants.TokenName, result.AccessToken ?? string.Empty);

            _http.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.AccessToken);

            var authenticatedUser = new ClaimsPrincipal(
                new ClaimsIdentity(
                    JwtParser.ParseClaimsFromPayload(result.AccessToken ?? string.Empty), 
                        AuthConstants.AuthenticationType));

            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));

            NotifyAuthenticationStateChanged(authState);

            return result;
        }
        catch
        {
            return default;
        }
    }

    public async Task Logout()
    {
        await _storage.RemoveAsync(AuthConstants.TokenName);

        _http.Client.DefaultRequestHeaders.Authorization = null;

        var authState = Task.FromResult(_anonymous);

        NotifyAuthenticationStateChanged(authState);
    }

    public async Task<SignUpUserDTO?> GetUserFromToken()
    {
        var token = await _storage.GetAsync(AuthConstants.TokenName);

        if (string.IsNullOrWhiteSpace(token)) return default;

        return JwtParser.ParseUserInfoFromPayload(token);
    }
}

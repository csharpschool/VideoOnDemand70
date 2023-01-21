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
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJWT(token), AuthConstants.AuthenticationType)));
    }
}

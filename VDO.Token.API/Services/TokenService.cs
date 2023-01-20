namespace VDO.Token.API.Services;

public class TokenService : ITokenService
{
    #region Properties
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;
    private readonly UserManager<VODUser> _userManager;
    #endregion

    #region Constructors
    public TokenService(IConfiguration configuration, IUserService userService, UserManager<VODUser> userManager)
    {
        _configuration = configuration;
        _userService = userService;
        _userManager = userManager;
    }
    #endregion

    #region Helper Methods
    private string? CreateToken(IList<string>? roles, VODUser user)
    {
        try
        {
            if (_configuration["Jwt:SigningSecret"] is null ||
               _configuration["Jwt:Duration"] is null ||
               _configuration["Jwt:Issuer"] is null ||
               _configuration["Jwt:Audience"] is null ||
               roles is null || user is null)
                throw new ArgumentException("JWT configuration missing.");

            var signingKey = Convert.FromBase64String(_configuration["Jwt:SigningSecret"] ?? "");
            var credentials = new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256Signature);
            var duration = int.Parse(_configuration["Jwt:Duration"] ?? "");
            var now = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString();
            var expires = new DateTimeOffset(DateTime.UtcNow.AddDays(duration)).ToUnixTimeSeconds().ToString();

            //Claim Types: https://datatracker.ietf.org/doc/html/rfc7519#section-4
            List<Claim> claims = new() {
                new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"] ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"] ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Nbf, now),
                new Claim(JwtRegisteredClaimNames.Exp, expires),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id)
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var jwtToken = new JwtSecurityToken(
                new JwtHeader(credentials),
                new JwtPayload(claims)
            );

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtTokenHandler.WriteToken(jwtToken);
            return token;
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region Token Methods
    public async Task<string?> GenerateTokenAsync(TokenUserDTO tokenUserDTO)
    {
        try
        {
            var user = await _userService.GetUserAsync(tokenUserDTO.Email);

            if (user is null) throw new UnauthorizedAccessException();

            var roles = await _userManager.GetRolesAsync(user);
            var token = CreateToken(roles, user);

            var result = await _userManager.SetAuthenticationTokenAsync(user, "VOD", "UserToken", token);

            if (result != IdentityResult.Success)
                throw new SecurityTokenException("Could not add token to user");

            return token;
        }
        catch
        {
            throw;
        }
    }

    public async Task<AuthenticatedUserDTO> GetTokenAsync(LoginUserDTO loginUserDTO)
    {
        try
        {
            if (loginUserDTO is null) throw new UnauthorizedAccessException();

            var user = await _userService.GetUserAsync(loginUserDTO);

            if (user is null) throw new UnauthorizedAccessException();

            var token = await _userManager.GetAuthenticationTokenAsync(user, "VOD", "UserToken");

            return new AuthenticatedUserDTO(token, user.UserName);
        }
        catch
        {
            throw;
        }
    }

    #endregion
}

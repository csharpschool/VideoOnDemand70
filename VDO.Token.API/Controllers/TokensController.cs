namespace VDO.Token.API.Controllers
{
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public TokensController(ITokenService tokenService) => _tokenService = tokenService;

        [Route("token")]
        [HttpPost]
        public async Task<IResult> Get([FromBody] LoginUserDTO loginUser)
        {
            try
            {
                var result = await _tokenService.GetTokenAsync(loginUser);

                if (string.IsNullOrWhiteSpace(result.AccessToken) ||
                    string.IsNullOrWhiteSpace(result.UserName)) return Results.Unauthorized();

                return Results.Ok(result);
            }
            catch
            {
            }

            return Results.Unauthorized();
        }

        [Route("token/create")]
        [HttpPost]
        public async Task<IResult> Create(TokenUserDTO tokenUserDTO)
        {
            try
            {
                var jwt = await _tokenService.GenerateTokenAsync(tokenUserDTO);
                if (string.IsNullOrWhiteSpace(jwt)) return Results.Unauthorized();
                return Results.Created("Token", jwt);
            }
            catch
            {
            }
            return Results.Unauthorized();
        }

    }
}

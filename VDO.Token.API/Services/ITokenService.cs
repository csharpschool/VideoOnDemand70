namespace VDO.Token.API.Services
{
    public interface ITokenService
    {
        Task<string?> GenerateTokenAsync(TokenUserDTO tokenUserDTO);
        Task<AuthenticatedUserDTO> GetTokenAsync(LoginUserDTO loginUserDTO);
    }
}
namespace VOD.Users.Database.Services;

public class UserService : IUserService
{
    private readonly UserManager<VODUser> _userManager;

    public UserService(UserManager<VODUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<VODUser?> GetUserAsync(string email)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }
        catch
        {
        }

        return default;
    }

    public async Task<VODUser?> GetUserAsync(LoginUserDTO loginUser)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(loginUser.Email);

            if (user is null) return default;

            var hasher = new PasswordHasher<VODUser>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, loginUser.Password);

            if (result.Equals(PasswordVerificationResult.Success)) return user;
        }
        catch
        {
        }

        return default;
    }
}

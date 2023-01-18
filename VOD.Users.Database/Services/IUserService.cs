using VOD.Common.DTOs;

namespace VOD.Users.Database.Services
{
    public interface IUserService
    {
        Task<VODUser?> GetUserAsync(LoginUserDTO loginUser);
        Task<VODUser?> GetUserAsync(string email);
    }
}
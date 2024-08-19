using user_api.Models;

namespace user_api.Services
{
    public interface IUserService
    {
        Task<UserModel?> FindUserByIdAsync(int id);
        Task<UserModel?> FindByEmailAsync(string email);
        Task<UserModel?> CreateUserAsync(UserModel user);
        Task<UserModel?> UpdateUserAsync(int id, UserModel user);
        Task<UserModel?> DeleteUserAsync(int id);
        Task<List<UserModel>> GetAllUsersAsync();
        Task<UserModel> AuthenticateUserAsync(string email, string password);
    }
}

using user_api.Models;

namespace user_api.Services
{
    public interface IJwtService
    {
        string GenerateToken(UserModel user);
    }
}

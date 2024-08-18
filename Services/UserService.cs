using user_api.Data;
using user_api.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace user_api.Services
{
    public class UserService(ApplicationDbContext context) : IUserService
    {
        public async Task<UserModel?> CreateUserAsync(UserModel user)
        {
            if (await FindByEmailAsync(user.Email) != null)
            {
                throw new Exception($"User by the email {user.Email} is already exists");
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<UserModel?> FindByEmailAsync(string email) =>
            await context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<List<UserModel>> GetAllUsersAsync() => 
            await context.Users.ToListAsync();

        public async Task<UserModel?> FindUserByIdAsync(int id) => 
            await context.Users.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<UserModel?> UpdateUserAsync(int id, UserModel user)
        {
            UserModel byId = await FindUserByIdAsync(id)
                ?? throw new Exception($"User by the id {id} doesnt exists");
            byId.Email = user.Email;
            byId.Name = user.Name;
            byId.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            await context.SaveChangesAsync();
            return byId;
        }

        public async Task<UserModel?> DeleteUserAsync(int id)
        {
            UserModel byId = await FindUserByIdAsync(id)
                ?? throw new Exception($"User by the id {id} doesnt exists");
            context.Users.Remove(byId);
            await context.SaveChangesAsync();
            return byId;
        }

        public async Task<bool> AuthenticateUserAsync(string email, string password)
        {
            var user = await FindByEmailAsync(email)
                ?? throw new Exception($"User by the email {email} does not exists");

            var res = BCrypt.Net.BCrypt.Verify(password, user.Password);

            return res;
        }
    }
}

namespace user_api.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public byte[]? Image { get; set; }
        public List<UserModel> Friends { get; set; } = [];
    }
}

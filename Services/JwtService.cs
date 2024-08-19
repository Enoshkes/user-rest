using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using user_api.Models;

namespace user_api.Services
{
    public class JwtService(IConfiguration configuration) : IJwtService
    {
        public string GenerateToken(UserModel user)
        {
            string key = configuration.GetValue("Jwt:Key", string.Empty)
                 ?? throw new ArgumentNullException("Key doesnt exists on JWT");

            int expiry = configuration.GetValue("Jwt:ExpiryInMinutes", 60);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims =
            {
                new (ClaimTypes.NameIdentifier, user.Name),
                new (ClaimTypes.Email , user.Email),
            };

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(expiry),
                claims: claims,
                signingCredentials: credentials
             );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}

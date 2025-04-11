using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CTFApp.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;


        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string userId, string username, string role)
        {
            var claims = new[]
            {
                new Claim("Id", userId),
                new Claim("Username", username),
                new Claim("Role", role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expireMin = int.Parse(_configuration["JwtSettings:ExpireMinutes"]);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(expireMin),
                signingCredentials: creds

                );


            return new JwtSecurityTokenHandler().WriteToken(token);


        }
    }
}

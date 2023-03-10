using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using westcoast_cars.api.Models;

namespace westcoast_cars.api.Services
{
    public class TokenService
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IConfiguration _config;
        public TokenService(UserManager<UserModel> userManager, IConfiguration config)
        {
            _config = config;
            _userManager = userManager;
        }

        public async Task<string> CreateToken(UserModel user)
        {
            // JWT Payload...
            var claims = new List<Claim>{
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            // --------------------PAYLOAD DONE----------------------

            //Signature....
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["tokenSettings:tokenKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var options = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddDays(5),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(options);
        }
    }
}
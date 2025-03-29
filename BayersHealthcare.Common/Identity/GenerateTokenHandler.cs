using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BayersHealthcare.Common.Identity
{
    public class GenerateTokenHandler
    {
        private readonly IConfiguration _configuration;

        public GenerateTokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string name, string role, string phone, string id)
        {
            List<Claim> claims = new()
            {
                new Claim(type: "Name", name),
                new Claim(ClaimTypes.Role, role),
                new Claim(type: "Id", id),
                new Claim(type: "Phone", phone)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("jwt")["Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                issuer: _configuration.GetSection("jwt")["Issuer"],
                audience: _configuration.GetSection("jwt")["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}


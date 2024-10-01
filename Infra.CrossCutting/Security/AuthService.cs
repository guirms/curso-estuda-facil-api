using Domain.Interfaces.Services;
using Domain.Utils.Constants;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infra.CrossCutting.Security
{
    public class AuthService : IAuthService
    {
        public string GenerateToken(int claimId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Token.JwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new(Token.NameIdentifier, claimId.ToString())
            };

            var securityToken = new JwtSecurityToken(
                Token.JwtIssuer,
                Token.JwtIssuer,
                claims,
                expires: DateTime.Now.AddHours(10),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}

using Microsoft.IdentityModel.Tokens;
using RoadReady.Interface;
using RoadReady.Models.DTOs;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RoadReady.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _keyString;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration configuration)
        {
            _keyString = configuration["SecretKey"].ToString();
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_keyString));
        }
        public async Task<string> GenerateToken(LoginValidationDto validation)
        {
            string token = string.Empty;
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId,validation.Username),
                new Claim(ClaimTypes.Role,validation.Role)
            };
            //Algorithm Signature with secret key
            var cred = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            //Giving the token decription
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred
            };

            //Putting the token together
            var tokenHandler = new JwtSecurityTokenHandler();
            var myToken = tokenHandler.CreateToken(tokenDescription);
            token = tokenHandler.WriteToken(myToken);
            return token;
        }
    }
}

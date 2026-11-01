using ECommerce.UserService.Models;
using ECommerce.UserService.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ECommerce.UserService.Services
{
    public class JWTGenerationService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public JWTGenerationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenResult GenerateToken(int userId, User.Role role)
        {
            var validatekey = _configuration["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(validatekey))
            {
                throw new InvalidOperationException("JWT key is not configured.");
            }

            byte[] keybytes;
            try
            {
                keybytes = Convert.FromBase64String(validatekey);

            }
            catch (FormatException)
            {
                throw new FormatException("JWT key is not a valid Base64 string.");
            }

            if(keybytes.Length < 32)
            {
                throw new InvalidOperationException("JWT key must be at least 256 bits (32 bytes) long.");
            }
            

            var key = new SymmetricSecurityKey(keybytes);
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            Collection<Claim> claims = new Collection<Claim>
            {
                new Claim( JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim( ClaimTypes.Role, role.ToString())    
            };

            var issuer = _configuration["Jwt:Issuer"];
            if (string.IsNullOrWhiteSpace(issuer))
            {
                
               throw new InvalidOperationException("JWT issuer is not configured.");
            }

            var audience = _configuration["Jwt:Audience"];
            if (string.IsNullOrWhiteSpace(audience))
            {
                throw new InvalidOperationException("JWT audience is not configured.");
            }

            var expiration = _configuration["Jwt:ExpireMinutes"];
            if(!int.TryParse(expiration, out int expireMinutes) || expireMinutes <= 0)
            {
                throw new InvalidOperationException("JWT expiration time is not configured or is not a valid positive integer.");
            }
            var expiretime = DateTime.UtcNow.AddMinutes(expireMinutes);
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiretime,
                signingCredentials: credentials
            );

            var tokenstring = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenResult
            {
                Token = tokenstring,
                ExpiresAt = expiretime
            };
        }
    }
}

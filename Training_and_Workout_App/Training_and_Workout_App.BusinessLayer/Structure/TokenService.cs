using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Training_and_Workout_App.BusinessLayer.Interfaces;

namespace Training_and_Workout_App.BusinessLayer.Structure;

public class TokenService(IConfiguration configuration) : ITokenService
{
    public string GenerateToken(int userId, string fullName, string role)
    {
        // 1. Cheia simetrica — citita din configuratie, NICIODATA hardcodata
        var keyBytes = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);
        var signingKey = new SymmetricSecurityKey(keyBytes);
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        // 2. Claims — informatiile puse in Payload
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()), // "sub"
            new Claim(ClaimTypes.Name, fullName),                    // "name"
            new Claim(ClaimTypes.Role, role)                         // "role" — folosit de [Authorize(Roles=...)]
        };

        // 3. Durata de expirare
        var expireMinutes = int.Parse(configuration["Jwt:ExpireMinutes"]!);
        var expires = DateTime.UtcNow.AddMinutes(expireMinutes);

        // 4. Construim token-ul
        var token = new JwtSecurityToken(
            issuer:             configuration["Jwt:Issuer"],
            audience:           configuration["Jwt:Audience"],
            claims:             claims,
            expires:            expires,
            signingCredentials: credentials
        );

        // 5. Serializam in format Header.Payload.Signature
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

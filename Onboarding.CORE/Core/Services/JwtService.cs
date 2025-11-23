using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Onboarding.CORE.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(string userId, string userName, string role)
    {
        // 📌 Priorizar variables de entorno > luego appsettings.json
        var secret = _config["JWT_SECRET"] ?? _config["Jwt:Secret"];
        var issuer = _config["JWT_ISSUER"] ?? _config["Jwt:Issuer"];
        var audience = _config["JWT_AUDIENCE"] ?? _config["Jwt:Audience"];

        if (string.IsNullOrEmpty(secret))
            throw new Exception("⚠ No se encontró JWT_SECRET ni Jwt:Secret en la configuración.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim("userName", userName),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

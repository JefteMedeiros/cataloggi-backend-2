using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using cataloggi_backend_2.DTOs.Auth;
using cataloggi_backend_2.Options;
using cataloggi_backend_2.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace cataloggi_backend_2.Services;

public class AuthService(IOptions<AuthOptions> authOptions) : IAuthService
{
    private readonly AuthOptions _options = authOptions.Value;

    public LoginResponseDto? Login(LoginRequestDto dto)
    {
        AuthOptionsValidator.Validate(_options);

        if (!CredentialsMatch(dto.Username, dto.Password))
            return null;

        var expiresAt = DateTime.UtcNow.AddHours(_options.TokenLifetimeHours);
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, _options.Username),
            new Claim(JwtRegisteredClaimNames.Sub, _options.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new LoginResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expiresAt
        };
    }

    private bool CredentialsMatch(string username, string password)
    {
        return FixedTimeEquals(username, _options.Username)
            && FixedTimeEquals(password, _options.Password);
    }

    private static bool FixedTimeEquals(string value, string expectedValue)
    {
        var valueBytes = Encoding.UTF8.GetBytes(value);
        var expectedValueBytes = Encoding.UTF8.GetBytes(expectedValue);

        return valueBytes.Length == expectedValueBytes.Length
            && CryptographicOperations.FixedTimeEquals(valueBytes, expectedValueBytes);
    }

}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Incoders.Template.Application.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Incoders.Template.Infrastructure.Identity;

internal sealed class JwtTokenService : IJwtTokenService
{
    private readonly IOptions<JwtOptions> _options;
    private readonly IClock _clock;
    private readonly RefreshTokenStore _refreshTokens;

    public JwtTokenService(IOptions<JwtOptions> options, IClock clock, RefreshTokenStore refreshTokens)
    {
        _options = options;
        _clock = clock;
        _refreshTokens = refreshTokens;
    }

    public TokenPair Issue(ApplicationUser user)
    {
        ArgumentNullException.ThrowIfNull(user);
        var opts = _options.Value;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        if (!string.IsNullOrWhiteSpace(user.Email))
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
        }

        var key = BuildSigningKey(opts.Secret);
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = _clock.UtcNow.AddMinutes(opts.AccessTokenMinutes);

        var token = new JwtSecurityToken(
            issuer: opts.Issuer,
            audience: opts.Audience,
            claims: claims,
            notBefore: _clock.UtcNow,
            expires: expires,
            signingCredentials: credentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshToken = GenerateRefreshToken();
        _refreshTokens.Store(refreshToken, user.Id, _clock.UtcNow.AddDays(opts.RefreshTokenDays));

        return new TokenPair(accessToken, refreshToken, expires);
    }

    public Guid? ConsumeRefreshToken(string refreshToken) =>
        _refreshTokens.Consume(refreshToken, _clock.UtcNow);

    internal static SymmetricSecurityKey BuildSigningKey(string secret)
    {
        if (string.IsNullOrWhiteSpace(secret))
        {
            throw new InvalidOperationException("Authentication:Jwt:Secret is not configured.");
        }

        return new SymmetricSecurityKey(SHA256.HashData(Encoding.UTF8.GetBytes(secret)));
    }

    private static string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(48);
        return Convert.ToBase64String(bytes);
    }
}

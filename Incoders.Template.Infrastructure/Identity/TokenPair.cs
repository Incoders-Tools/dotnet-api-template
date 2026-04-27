namespace Incoders.Template.Infrastructure.Identity;

public sealed record TokenPair(string AccessToken, string RefreshToken, DateTime AccessTokenExpiresAtUtc);

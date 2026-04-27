namespace Incoders.Template.Infrastructure.Identity;

public sealed class JwtOptions
{
    public const string SectionName = "Authentication:Jwt";

    public string Issuer { get; set; } = "Incoders.Template";

    public string Audience { get; set; } = "Incoders.Template";

    public string Secret { get; set; } = string.Empty;

    public int AccessTokenMinutes { get; set; } = 30;

    public int RefreshTokenDays { get; set; } = 7;
}

namespace Incoders.Template.Infrastructure.Identity;

public interface IJwtTokenService
{
    TokenPair Issue(ApplicationUser user);

    Guid? ConsumeRefreshToken(string refreshToken);
}

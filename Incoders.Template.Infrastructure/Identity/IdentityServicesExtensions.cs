using Incoders.Template.Application.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Incoders.Template.Infrastructure.Identity;

public static class IdentityServicesExtensions
{
    /// <summary>
    /// Registers ASP.NET Core Identity (EF Core in-memory) and JWT Bearer authentication.
    /// Swap <c>UseInMemoryDatabase</c> for <c>UseNpgsql</c>/<c>UseSqlServer</c> to persist.
    /// </summary>
    public static IServiceCollection AddIdentityAndJwt(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .Validate(o => !string.IsNullOrWhiteSpace(o.Secret), "Authentication:Jwt:Secret is required.")
            .Validate(o => o.AccessTokenMinutes > 0, "Authentication:Jwt:AccessTokenMinutes must be positive.")
            .Validate(o => o.RefreshTokenDays > 0, "Authentication:Jwt:RefreshTokenDays must be positive.")
            .ValidateOnStart();

        services.AddDbContext<AppIdentityDbContext>(opts =>
            opts.UseInMemoryDatabase("IncodersTemplateIdentity"));

        services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();

        services.AddSingleton<RefreshTokenStore>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        services.Replace(ServiceDescriptor.Scoped<ICurrentUser, HttpContextCurrentUser>());

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, _ => { })
            .Services
            .AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IOptions<JwtOptions>>((bearer, jwt) =>
            {
                var opts = jwt.Value;
                bearer.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = opts.Issuer,
                    ValidAudience = opts.Audience,
                    IssuerSigningKey = JwtTokenService.BuildSigningKey(opts.Secret),
                    ClockSkew = TimeSpan.FromSeconds(30),
                };
            });

        services.AddAuthorization();

        return services;
    }
}

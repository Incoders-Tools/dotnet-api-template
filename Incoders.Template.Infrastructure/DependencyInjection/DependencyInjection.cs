using Incoders.Template.Application.Abstractions;
//#if (includeExternalApiExample)
using Incoders.Template.Application.Abstractions.ExternalServices;
//#endif
//#if (multiTenant)
using Incoders.Template.Application.Abstractions.Tenancy;
//#endif
using Incoders.Template.Infrastructure.Caching.InMemory;
//#if (includeExternalApiExample)
using Incoders.Template.Infrastructure.ExternalServices;
//#endif
//#if (includeAuth)
using Incoders.Template.Infrastructure.Identity;
//#endif
using Incoders.Template.Infrastructure.Persistence.InMemory;
//#if (multiTenant)
using Incoders.Template.Infrastructure.Tenancy;
//#endif
using Incoders.Template.Infrastructure.Time;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
//#if (includeExternalApiExample)
using Microsoft.Extensions.Options;
//#endif

namespace Incoders.Template.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.TryAddTimeProvider();
        services.AddHttpContextAccessor();

        services.AddSingleton<InMemorySystemSettingStore>();
        services.AddScoped<ISystemSettingRepository, InMemorySystemSettingRepository>();
        services.AddScoped<IUnitOfWork, InMemoryUnitOfWork>();

        services.AddSingleton<ICacheStore, InMemoryCacheStore>();
        services.AddSingleton<IClock, SystemClock>();

        services.TryAddScoped<ICurrentUser, NullCurrentUser>();

        //#if (includeAuth)
        services.AddIdentityAndJwt(configuration);
        //#endif

        //#if (multiTenant)
        services.AddMultiTenancy(configuration);
        //#endif

        //#if (includeExternalApiExample)
        services.AddExchangeRateGateway(configuration);
        //#endif

        return services;
    }

    //#if (multiTenant)
    private static IServiceCollection AddMultiTenancy(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<MultiTenancyOptions>()
            .Bind(configuration.GetSection(MultiTenancyOptions.SectionName));

        services.AddSingleton<InMemoryTenantStore>();
        services.AddScoped<ITenantRepository, InMemoryTenantRepository>();
        services.AddSingleton<ITenantMembershipService, InMemoryTenantMembershipService>();
        services.AddScoped<CurrentTenant>();
        services.AddScoped<ICurrentTenant>(sp => sp.GetRequiredService<CurrentTenant>());
        services.AddScoped<ITenantResolver, HeaderTenantResolver>();

        return services;
    }
    //#endif

    //#if (includeExternalApiExample)
    private static IServiceCollection AddExchangeRateGateway(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<ExchangeRateOptions>()
            .Bind(configuration.GetSection(ExchangeRateOptions.SectionName))
            .Validate(
                o => !string.IsNullOrWhiteSpace(o.BaseUrl) && Uri.TryCreate(o.BaseUrl, UriKind.Absolute, out _),
                $"{ExchangeRateOptions.SectionName}:BaseUrl must be a valid absolute URL.")
            .ValidateOnStart();

        services.AddHttpClient<ExchangeRateApiClient>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<ExchangeRateOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
        });

        services.AddScoped<IExchangeRateGateway, ExchangeRateGateway>();

        return services;
    }
    //#endif

    private static void TryAddTimeProvider(this IServiceCollection services)
    {
        if (services.Any(d => d.ServiceType == typeof(TimeProvider)))
        {
            return;
        }

        services.AddSingleton(TimeProvider.System);
    }
}

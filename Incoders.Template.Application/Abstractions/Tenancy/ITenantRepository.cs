using Incoders.Template.Domain.Tenants;

namespace Incoders.Template.Application.Abstractions.Tenancy;

public interface ITenantRepository
{
    Task<Tenant?> GetByIdAsync(TenantId id, CancellationToken cancellationToken = default);

    Task AddAsync(Tenant tenant, CancellationToken cancellationToken = default);
}

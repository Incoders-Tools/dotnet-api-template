using Incoders.Template.Application.Abstractions.Tenancy;
using Incoders.Template.Domain.Tenants;

namespace Incoders.Template.Infrastructure.Tenancy;

internal sealed class InMemoryTenantRepository : ITenantRepository
{
    private readonly InMemoryTenantStore _store;

    public InMemoryTenantRepository(InMemoryTenantStore store)
    {
        _store = store;
    }

    public Task<Tenant?> GetByIdAsync(TenantId id, CancellationToken cancellationToken = default)
    {
        _store.Tenants.TryGetValue(id, out var tenant);
        return Task.FromResult(tenant);
    }

    public Task AddAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        if (!_store.Tenants.TryAdd(tenant.Id, tenant))
        {
            throw new InvalidOperationException($"Tenant '{tenant.Id}' already exists.");
        }
        return Task.CompletedTask;
    }
}

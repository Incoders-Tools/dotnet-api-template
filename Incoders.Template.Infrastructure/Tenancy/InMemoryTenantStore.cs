using System.Collections.Concurrent;
using Incoders.Template.Domain.Tenants;

namespace Incoders.Template.Infrastructure.Tenancy;

public sealed class InMemoryTenantStore
{
    internal ConcurrentDictionary<TenantId, Tenant> Tenants { get; } = new();

    internal ConcurrentBag<TenantMembership> Memberships { get; } = new();
}

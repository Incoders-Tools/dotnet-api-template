using Incoders.Template.Domain.Tenants;

namespace Incoders.Template.Application.Features.Tenants;

public sealed record TenantResponse(
    Guid Id,
    string Name,
    Guid OwnerUserId,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc)
{
    public static TenantResponse FromDomain(Tenant tenant) =>
        new(tenant.Id.Value, tenant.Name, tenant.OwnerUserId, tenant.CreatedAtUtc, tenant.UpdatedAtUtc);
}

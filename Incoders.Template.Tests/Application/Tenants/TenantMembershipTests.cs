using Incoders.Template.Domain.Tenants;
using Incoders.Template.Infrastructure.Tenancy;

namespace Incoders.Template.Tests.Application.Tenants;

public class TenantMembershipTests
{
    private static readonly DateTime Now = new(2026, 4, 24, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public async Task InMemoryService_TracksMembershipPerTenant()
    {
        var store = new InMemoryTenantStore();
        var service = new InMemoryTenantMembershipService(store);
        var tenantA = TenantId.New();
        var tenantB = TenantId.New();
        var user = Guid.NewGuid();

        await service.AddAsync(new TenantMembership(tenantA, user, TenantRole.Owner, Now));

        Assert.True(await service.IsMemberAsync(tenantA, user));
        Assert.False(await service.IsMemberAsync(tenantB, user));
        Assert.Equal(TenantRole.Owner, await service.GetRoleAsync(tenantA, user));
    }

    [Fact]
    public async Task InMemoryService_ListsAllMembershipsForUser()
    {
        var store = new InMemoryTenantStore();
        var service = new InMemoryTenantMembershipService(store);
        var user = Guid.NewGuid();
        var t1 = TenantId.New();
        var t2 = TenantId.New();

        await service.AddAsync(new TenantMembership(t1, user, TenantRole.Owner, Now));
        await service.AddAsync(new TenantMembership(t2, user, TenantRole.Member, Now));

        var list = await service.ListForUserAsync(user);
        Assert.Equal(2, list.Count);
        Assert.Contains(list, m => m.TenantId == t1 && m.Role == TenantRole.Owner);
        Assert.Contains(list, m => m.TenantId == t2 && m.Role == TenantRole.Member);
    }
}

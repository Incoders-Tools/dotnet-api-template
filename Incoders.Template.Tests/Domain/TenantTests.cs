using Incoders.Template.Domain.Tenants;

namespace Incoders.Template.Tests.Domain;

public class TenantTests
{
    private static readonly DateTime Now = new(2026, 4, 24, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void Create_Succeeds_WithValidInput()
    {
        var userId = Guid.NewGuid();
        var result = Tenant.Create(TenantId.New(), "Acme Corp", userId, Now);
        Assert.True(result.IsSuccess);
        Assert.Equal("Acme Corp", result.Value.Name);
        Assert.Equal(userId, result.Value.OwnerUserId);
    }

    [Fact]
    public void Create_RejectsEmptyName()
    {
        var result = Tenant.Create(TenantId.New(), "   ", Guid.NewGuid(), Now);
        Assert.True(result.IsFailure);
        Assert.Equal("tenants.name_required", result.Error.Code);
    }

    [Fact]
    public void Create_RejectsEmptyOwner()
    {
        var result = Tenant.Create(TenantId.New(), "Acme", Guid.Empty, Now);
        Assert.True(result.IsFailure);
        Assert.Equal("tenants.owner_required", result.Error.Code);
    }
}

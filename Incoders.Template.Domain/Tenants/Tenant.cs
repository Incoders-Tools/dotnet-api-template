using Incoders.Template.Domain.Common;

namespace Incoders.Template.Domain.Tenants;

/// <summary>
/// Aggregate root representing a tenant / organization in the shared-database, tenant-column isolation model.
/// </summary>
public sealed class Tenant : Entity<TenantId>
{
    public const int NameMaxLength = 200;

    private Tenant(TenantId id, string name, Guid ownerUserId, DateTime createdAtUtc, DateTime updatedAtUtc)
        : base(id)
    {
        Name = name;
        OwnerUserId = ownerUserId;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = updatedAtUtc;
    }

    public string Name { get; private set; }

    public Guid OwnerUserId { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime UpdatedAtUtc { get; private set; }

    public static Result<Tenant> Create(TenantId id, string name, Guid ownerUserId, DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Tenant>(TenantErrors.NameRequired);
        }

        if (ownerUserId == Guid.Empty)
        {
            return Result.Failure<Tenant>(TenantErrors.OwnerRequired);
        }

        return Result.Success(new Tenant(id, name.Trim(), ownerUserId, nowUtc, nowUtc));
    }

    public Result Rename(string name, DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure(TenantErrors.NameRequired);
        }

        Name = name.Trim();
        UpdatedAtUtc = nowUtc;
        return Result.Success();
    }
}

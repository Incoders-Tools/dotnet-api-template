using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Incoders.Template.Infrastructure.Identity;

/// <summary>
/// Identity persistence context. Defaults to the in-memory EF Core provider; swap to Npgsql/SqlServer in <c>AddIdentityAndJwt</c>.
/// </summary>
public sealed class AppIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options) { }
}

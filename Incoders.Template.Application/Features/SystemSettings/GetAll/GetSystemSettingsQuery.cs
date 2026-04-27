using Incoders.Template.Application.Common.Pagination;
using Incoders.Template.Application.Features.SystemSettings;
using Incoders.Template.Domain.Common;
using Incoders.Template.Domain.SystemSettings;
using MediatR;

namespace Incoders.Template.Application.Features.SystemSettings.GetAll;

public sealed record GetSystemSettingsQuery(SystemSettingsFilter Filter)
    : IRequest<Result<PageResult<SystemSettingResponse>>>;

/// <summary>
/// Transport-level filter. The API layer may override <see cref="TenantId"/> based on the resolved tenant.
/// </summary>
public sealed record SystemSettingsFilter(
    SystemSettingScope? Scope = null,
    Guid? TenantId = null,
    Guid? UserId = null,
    string? Key = null,
    int Page = 1,
    int PageSize = 20);

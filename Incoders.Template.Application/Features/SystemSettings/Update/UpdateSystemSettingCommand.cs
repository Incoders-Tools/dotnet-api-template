using Incoders.Template.Application.Features.SystemSettings;
using Incoders.Template.Domain.Common;
using Incoders.Template.Domain.SystemSettings;
using MediatR;

namespace Incoders.Template.Application.Features.SystemSettings.Update;

public sealed record UpdateSystemSettingCommand(
    Guid Id,
    string Value,
    SystemSettingScope Scope,
    Guid? TenantId,
    Guid? UserId,
    Guid? RequiredTenantId = null) : IRequest<Result<SystemSettingResponse>>;

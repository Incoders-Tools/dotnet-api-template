using Incoders.Template.Application.Features.SystemSettings;
using Incoders.Template.Domain.Common;
using MediatR;

namespace Incoders.Template.Application.Features.SystemSettings.GetById;

public sealed record GetSystemSettingByIdQuery(Guid Id, Guid? RequiredTenantId = null)
    : IRequest<Result<SystemSettingResponse>>;

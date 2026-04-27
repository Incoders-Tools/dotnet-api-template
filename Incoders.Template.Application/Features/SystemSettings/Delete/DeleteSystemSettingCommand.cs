using Incoders.Template.Domain.Common;
using MediatR;

namespace Incoders.Template.Application.Features.SystemSettings.Delete;

public sealed record DeleteSystemSettingCommand(Guid Id, Guid? RequiredTenantId = null) : IRequest<Result>;

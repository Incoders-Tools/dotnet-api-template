using Incoders.Template.Domain.Common;
using MediatR;

namespace Incoders.Template.Application.Features.Tenants;

public sealed record CreateTenantCommand(string Name) : IRequest<Result<TenantResponse>>;

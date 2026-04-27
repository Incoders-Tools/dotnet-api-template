using FluentValidation;
using Incoders.Template.Domain.Tenants;

namespace Incoders.Template.Application.Features.Tenants;

public sealed class CreateTenantValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("tenants.errors.name_required")
            .MaximumLength(Tenant.NameMaxLength).WithMessage("tenants.errors.name_too_long");
    }
}

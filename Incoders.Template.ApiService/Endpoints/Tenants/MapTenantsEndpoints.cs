using Incoders.Template.ApiService.Endpoints.Common;
using Incoders.Template.Application.Features.Tenants;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Incoders.Template.ApiService.Endpoints.Tenants;

/// <summary>
/// Organization / tenant management endpoints. Available when multi-tenancy is enabled.
/// </summary>
public static class TenantsEndpointsExtensions
{
    public static IEndpointRouteBuilder MapTenantsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tenants")
            .WithTags("Tenants");
        //#if (includeAuth)
        group.RequireAuthorization();
        //#endif

        group.MapPost("/", CreateAsync)
            .WithName("CreateTenant")
            .WithSummary("Create a tenant / organization; the current user is added as Owner")
            .Produces<TenantResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        return app;
    }

    private static async Task<IResult> CreateAsync(
        [FromBody] CreateTenantRequest body,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CreateTenantCommand(body.Name), cancellationToken);
        return result.Match(tenant => TypedResults.Created($"/api/tenants/{tenant.Id}", tenant));
    }
}

public sealed record CreateTenantRequest(string Name);

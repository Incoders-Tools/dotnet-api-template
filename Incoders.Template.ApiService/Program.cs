using Incoders.Template.ApiService.Endpoints.Common;
//#if (includeExternalApiExample)
using Incoders.Template.ApiService.Endpoints.External;
//#endif
using Incoders.Template.ApiService.Endpoints.SystemSettings;
//#if (multiTenant)
using Incoders.Template.ApiService.Endpoints.Tenants;
//#endif
//#if (includeAuth)
using Incoders.Template.ApiService.Endpoints.Auth;
//#endif
using Incoders.Template.Application;
using Incoders.Template.Infrastructure.DependencyInjection;
//#if (multiTenant)
using Incoders.Template.Infrastructure.Tenancy;
//#endif

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//#if (multiTenant)
app.UseTenantResolution();
//#endif
//#if (includeAuth)
app.UseAuthentication();
app.UseAuthorization();
//#endif

app.MapGet("/", () => "API service is running.");

app.MapSystemSettingsEndpoints();
//#if (multiTenant)
app.MapTenantsEndpoints();
//#endif
//#if (includeExternalApiExample)
app.MapExternalEndpoints();
//#endif
//#if (includeAuth)
app.MapAuthEndpoints();
//#endif

app.MapDefaultEndpoints();

app.Run();

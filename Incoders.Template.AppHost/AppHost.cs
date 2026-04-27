var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Incoders_Template_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

//#if (includeWeb)
builder.AddProject<Projects.Incoders_Template_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);
//#endif

builder.Build().Run();

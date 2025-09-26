// Required namespaces
using Stimpon.Community.Api.Extensions;

WebApplication.CreateBuilder(args)
    .SetupServices()     // Setup services
    .Build()             // Build the application
    .SetupApplication()  // Configure the application
    .Run();              // Run the application

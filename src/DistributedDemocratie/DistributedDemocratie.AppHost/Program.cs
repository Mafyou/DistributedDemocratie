var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.DistributedDemocratie>("distributeddemocratie");

builder.Build().Run();

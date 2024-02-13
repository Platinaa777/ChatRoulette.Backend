using AuthService.Infrastructure.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json")
    .AddEnvironmentVariables()
    .Build();

builder.AddJwtAuthentication(builder.Configuration.GetSection("Jwt:Key").Value!);
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

await app.UseOcelot();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
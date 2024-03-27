using AuthService.Infrastructure.Extensions;
using AuthService.Infrastructure.Extensions.Jwt;
using Ocelot.DependencyInjection;
using Ocelot.Gateway.Middlewares;
using Ocelot.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json")
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddCors(corsOptions =>
{
    corsOptions.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Host.UseSerilog((ctx, config) =>
{
    config.ReadFrom.Configuration(ctx.Configuration);
});

builder.AddJwtAuthentication(builder.Configuration.GetSection("Jwt:Key").Value!);
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddSingleton<LoggingRequestMiddleware>();

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

await app.UseOcelot();
app.UseMiddleware<LoggingRequestMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
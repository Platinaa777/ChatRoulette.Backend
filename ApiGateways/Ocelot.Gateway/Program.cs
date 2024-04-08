using AuthService.Infrastructure.Extensions.Jwt;
using Ocelot.DependencyInjection;
using Ocelot.Gateway.Middlewares;
using Ocelot.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Configuration.GetSection("ENV").Value;
Console.WriteLine(env);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile($"ocelot.{env}.json")
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
builder.Services.AddSignalR();
builder.Services.AddSingleton<LoggingRequestMiddleware>();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseMiddleware<LoggingRequestMiddleware>();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

var configuration = new OcelotPipelineConfiguration
{
    AuthorizationMiddleware = async (context, next) =>
    {
        await next.Invoke();
    }
};

app.UseWebSockets();
await app.UseOcelot(configuration);

app.UseAuthentication();

app.Run();
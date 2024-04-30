using AuthService.Infrastructure.Extensions.Jwt;
using Ocelot.DependencyInjection;
using Ocelot.Gateway.Middlewares;
using Ocelot.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Configuration.GetSection("ENV").Value;
Console.WriteLine(env);

var cfg = builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile($"ocelot.{env}.json")
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddCors(corsOptions =>
{
    corsOptions.AddPolicy("frontend",x =>
        x.WithOrigins("http://82.146.62.254","http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Host.UseSerilog((ctx, config) =>
{
    config.ReadFrom.Configuration(ctx.Configuration);
});

builder.AddJwtAuthentication(builder.Configuration.GetSection("Jwt:Key").Value!);
builder.Services.AddOcelot(cfg);
builder.Services.AddSignalR();
builder.Services.AddSingleton<LoggingRequestMiddleware>();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseMiddleware<LoggingRequestMiddleware>();

app.UseCors("frontend");

var configuration = new OcelotPipelineConfiguration
{
    AuthorizationMiddleware = async (context, next) =>
    {
        await next.Invoke();
    }
};

app.UseAuthentication();
app.MapControllers();
app.UseWebSockets();
await app.UseOcelot(configuration);

app.Run();
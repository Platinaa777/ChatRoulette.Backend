using Serilog;
using Yarp.Proxy.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Host.UseSerilog((ctx, config) =>
{
    config.ReadFrom.Configuration(ctx.Configuration);
});
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddSingleton<LoggingRequestMiddleware>();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseMiddleware<LoggingRequestMiddleware>();
app.UseHttpsRedirection();

app.MapReverseProxy();

app.Run();
using Chat.Api.GrpcServices;
using Chat.Api.Infrastructure;
using Chat.Api.WebSockets;
using MassTransit.Client.Extensions;
using Otlp.Configuration.Extensions;
using Serilog;
using SwaggerConfigurations.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddDataLayer();
builder.AddCors();
builder.AddLoggingWithSerilog();
builder.AddChatMessaging();
builder.AddSwagger();
builder.AddEventBusClient();
builder.AddMassTransit();
builder.AddApplicationServices();
builder.AddMetricsAndTracing();
builder.AddBackgroundJobs();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseRouting();
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials());

app.UseEndpoints(routes =>
{
    routes.MapHub<ChatHub>("/chat");
});

app.MapControllers();

app.Run();
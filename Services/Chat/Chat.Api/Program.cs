using Chat.Api.Infrastructure;
using Chat.Api.WebSockets;
using MassTransit.Client.Extensions;
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

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseRouting();
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials());

#pragma warning disable ASP0014
app.UseEndpoints(routes =>
{
    routes.MapHub<ChatHub>("/my-chat");
});
#pragma warning restore ASP0014

app.MapControllers();

app.Run();
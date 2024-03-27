using MassTransit.Client.Extensions;
using ProfileService.Api.Extensions;
using Serilog;
using SwaggerConfigurations.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((ctx, config) =>
{
    config.ReadFrom.Configuration(ctx.Configuration);
});

builder.AddApplicationServices();
builder.AddDataLayer();
builder.AddSwagger();
builder.AddMassTransit();
builder.AddEventBusClient();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.MapControllers();

app.Run();
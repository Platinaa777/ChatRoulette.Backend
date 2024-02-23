using MassTransit.Client.Extensions;
using ProfileService.Api.Extensions;
using SwaggerConfigurations.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();
builder.AddDataLayer();
builder.AddSwagger();
builder.AddMassTransit();
builder.AddEventBusClient();

var app = builder.Build();

app.MapControllers();

app.Run();
using EmailingService.Api.Infrastructure;
using MassTransit.Client.Extensions;
using SwaggerConfigurations.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.AddSwagger();
builder.AddEventBusClient();
builder.AddEmailConfig();
builder.AddMassTransit();
builder.AddCacheRedis();

var app = builder.Build();

app.MapControllers();

app.Run();
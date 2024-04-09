using dotenv.net;
using EmailingService.Api.Infrastructure;
using MassTransit.Client.Extensions;
using Serilog;
using SwaggerConfigurations.Extensions;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationLayer();
builder.AddLoggingWithSerilog();
builder.AddSwagger();
builder.AddEventBusClient();
builder.AddEmailConfig();
builder.AddMassTransit();
builder.AddCacheRedis();

var app = builder.Build();

app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();
using dotenv.net;
using MassTransit.Client.Extensions;
using ProfileService.Api.Extensions;
using Serilog;
using SwaggerConfigurations.Extensions;

DotEnv.Load();
var builder = WebApplication.CreateBuilder(args);

builder.AddLoggingWithSerilog();
builder.AddApplicationServices();
builder.AddDataLayer();
builder.AddSwagger();
builder.AddMassTransit();
builder.AddEventBusClient();
builder.AddBackgroundJobs();
builder.AddS3Storage();
builder.AddRedisCache();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.MapControllers();

app.Run();
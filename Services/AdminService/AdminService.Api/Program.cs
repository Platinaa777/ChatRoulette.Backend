using AdminService.Api.Extensions;
using MassTransit.Client.Extensions;
using Serilog;
using SwaggerConfigurations.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddSwagger();
builder.AddApplicationServices();
builder.AddDataLayer();
builder.AddLoggingWithSerilog();
builder.AddEventBusClient();
builder.AddMassTransit();
builder.AddBackgroundJobs();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.MapControllers();

app.Run();
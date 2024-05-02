using dotenv.net;
using EmailingService.Api.Infrastructure;
using MassTransit.Client.Extensions;
using Otlp.Configuration.Extensions;
using Serilog;
using SwaggerConfigurations.Extensions;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(corsOptions =>
{
    corsOptions.AddPolicy("frontend",x =>
        x.WithOrigins("https://langskillup.ru","http://82.146.62.254","http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.AddApplicationLayer();
builder.AddLoggingWithSerilog();
builder.AddSwagger();
builder.AddEventBusClient();
builder.AddEmailConfig();
builder.AddMassTransit();
builder.AddCacheRedis();
builder.AddMetricsAndTracing();

var app = builder.Build();

app.UseCors("frontend");

app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();
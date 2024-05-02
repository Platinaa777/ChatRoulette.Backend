using dotenv.net;
using MassTransit.Client.Extensions;
using Otlp.Configuration.Extensions;
using ProfileService.Api.Extensions;
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

builder.AddLoggingWithSerilog();
builder.AddApplicationServices();
builder.AddDataLayer();
builder.AddSwagger();
builder.AddMassTransit();
builder.AddEventBusClient();
builder.AddBackgroundJobs();
builder.AddS3Storage();
builder.AddRedisCache();
builder.AddMetricsAndTracing();
builder.AddHttpChatClient();

var app = builder.Build();

app.UseCors("frontend");

app.UseSerilogRequestLogging();
app.MapControllers();

app.Run();
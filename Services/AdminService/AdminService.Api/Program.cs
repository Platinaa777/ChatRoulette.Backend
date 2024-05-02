using AdminService.Api.Extensions;
using MassTransit.Client.Extensions;
using Otlp.Configuration.Extensions;
using Serilog;
using SwaggerConfigurations.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(corsOptions =>
{
    corsOptions.AddPolicy("frontend",x =>
        x.WithOrigins("https://langskillup.ru","http://82.146.62.254","http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.AddSwagger();
builder.AddApplicationServices();
builder.AddDataLayer();
builder.AddLoggingWithSerilog();
builder.AddEventBusClient();
builder.AddMassTransit();
builder.AddBackgroundJobs();
builder.AddMetricsAndTracing();

var app = builder.Build();

app.UseCors("frontend");

app.UseSerilogRequestLogging();
app.MapControllers();

app.Run();
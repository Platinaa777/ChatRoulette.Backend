using AuthService.Api.Infrastructure;
using AuthService.Infrastructure.Extensions.UsersSeed;
using MassTransit.Client.Extensions;
using Serilog;
using SwaggerConfigurations.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddCors();
builder.AddSecurity();
builder.AddDataLayer();
builder.AddApplicationServices();
builder.AddSwagger();
builder.AddUsersSeed();
builder.AddEventBusClient();
builder.AddMassTransit();
builder.AddCacheUserConfirmation();
builder.AddLoggingWithSerilog();
builder.AddBackgroundJobs();
builder.AddOpenTelemetryTracing();
        
var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseCors("frontend");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
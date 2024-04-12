using AuthService.Api.Infrastructure;
using AuthService.Infrastructure.Extensions.UsersSeed;
using MassTransit.Client.Extensions;
using Otlp.Configuration.Extensions;
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
builder.AddMetricsAndTracing();
        
var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
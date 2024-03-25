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

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
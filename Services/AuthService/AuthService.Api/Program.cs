using AuthService.Api.Infrastructure;
using AuthService.Infrastructure.Extensions.UsersSeed;
using MassTransit.Client.Extensions;
using SwaggerConfigurations.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddSecurity();
builder.AddDataLayer();
builder.AddApplicationServices();
builder.AddSwagger();
builder.AddUsersSeed();
builder.AddEventBusClient();
builder.AddMassTransit();
builder.AddCacheUserConfirmation();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
using AuthService;
using AuthService.Extensions;
using SwaggerConfigurations.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.AddJwtAuthentication();
builder.Services.AddSingleton<JwtTokenHandler>();
builder.AddSwagger();

var app = builder.Build();

app.MapControllers();

app.Run();
using SwaggerConfigurations.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddSwagger();
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
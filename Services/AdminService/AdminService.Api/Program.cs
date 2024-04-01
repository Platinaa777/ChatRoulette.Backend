using SwaggerConfigurations.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddSwagger();

var app = builder.Build();


app.Run();
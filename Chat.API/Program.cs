using Chat.Application.Handlers;
using Chat.Core.Repositories;
using Chat.Core.Secrets;
using Chat.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(corsOptions =>
{
    corsOptions.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<TwilioSettings>();
builder.Services.AddSingleton<RoomHandler>();
builder.Services.AddSingleton<IRoomRepository, GoRoomRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(cfg =>
    {
        cfg.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        cfg.RoutePrefix = string.Empty;
    });
}

app.UseCors();
app.MapControllers();

app.Run();
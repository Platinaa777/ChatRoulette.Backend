using Chat.Api.Infrastructure;
using Chat.Api.WebSockets;
using Chat.Application.Commands.ConnectUser;
using Chat.Domain.Repositories;
using Chat.Infrastructure.Repositories;
using MassTransit.Client.Extensions;
using Serilog;
using SwaggerConfigurations.Extensions;

var builder = WebApplication.CreateBuilder(args);

var cfg = builder.Configuration;

builder.AddDataLayer();

builder.Services.AddCors(corsOptions =>
{
    corsOptions.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Host.UseSerilog((ctx, config) =>
{
    config.ReadFrom.Configuration(ctx.Configuration);
});

builder.Services.AddSignalR(options =>
{
    options.ClientTimeoutInterval = TimeSpan.MaxValue;
});

builder.AddSwagger();
builder.Services.AddTransient<ChatHub>();
builder.Services.AddMediatR(cfg 
    => cfg.RegisterServicesFromAssemblyContaining<ConnectUserCommand>());
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IChatUserRepository, ChatUserRepository>();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.AddEventBusClient();
builder.AddMassTransit();

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(cfg =>
    {
        cfg.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        cfg.RoutePrefix = string.Empty;
    });
}

app.UseRouting();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.UseEndpoints(routes =>
{
    routes.MapHub<ChatHub>("/my-chat");
});

app.MapControllers();

app.Run();
using Chat.Api.WebSockets;
using Chat.Application.Commands.ConnectUser;
using Chat.Domain.Repositories;
using Chat.Infrastructure.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var cfg = builder.Configuration;

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

// builder.AddSwagger();
builder.Services.AddTransient<ChatHub>();
builder.Services.AddMediatR(cfg 
    => cfg.RegisterServicesFromAssemblyContaining<ConnectUserCommand>());
builder.Services.AddSingleton<IRoomRepository, RoomRepository>();
builder.Services.AddSingleton<IChatUserRepository, ChatUserRepository>();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// app.UseAuthentication();
// app.UseAuthorization();

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
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/my-chat");
});
app.MapControllers();

app.Run();
using AuthService.Application.Handlers;
using AuthService.DataContext.Database;
using AuthService.Domain.JwtConfig;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Infrastructure.Extensions.Jwt;
using AuthService.Infrastructure.Extensions.UsersSeed;
using AuthService.Infrastructure.JwtGenerator;
using AuthService.Infrastructure.Repos;
using Microsoft.EntityFrameworkCore;
using SwaggerConfigurations.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<JwtTokenCreator>();

builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssemblyContaining<CreateUserCommandHandler>());

builder.Services.AddDbContext<UserDb>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("PostgreSQL"),
        b => b.MigrationsAssembly("AuthService.Migrations"));
});


builder.Services.Configure<Jwt>(configuration.GetSection("Jwt"));
builder.AddJwtAuthentication(configuration.GetSection("Jwt:Key").Value!);

builder.AddSwagger();
builder.AddUsersSeed();

var app = builder.Build();


app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
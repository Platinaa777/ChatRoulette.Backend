using AuthService.Application.Services;
using AuthService.DataContext.Database;
using AuthService.Domain.JwtConfig;
using AuthService.Domain.Models;
using AuthService.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SwaggerConfigurations.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddIdentity<UserAccount, IdentityRole>(opt =>
    {
        opt.Password.RequireDigit = false;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequireUppercase = false;
        opt.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<UserDb>();

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
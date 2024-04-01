using AdminService.DataContext.Database;
using AdminService.Domain.Models.ComplaintAggregate.Repos;
using AdminService.Infrastructure.Repos;
using DomainDriverDesignAbstractions;
using Microsoft.EntityFrameworkCore;
using SwaggerConfigurations.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddSwagger();
builder.Services.AddScoped<IComplaintRepository, ComplaintRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("PostgresSQL"),
        b => b.MigrationsAssembly("AdminService.DataContext"));
});

var app = builder.Build();


app.Run();
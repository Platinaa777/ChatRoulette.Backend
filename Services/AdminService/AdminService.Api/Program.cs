using AdminService.Application.Assembly;
using AdminService.Application.Behaviors;
using AdminService.Application.Commands.AddComplaintCommand;
using AdminService.DataContext.Database;
using AdminService.Domain.Models.ComplaintAggregate.Repos;
using AdminService.Infrastructure.Repos;
using DomainDriverDesignAbstractions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SwaggerConfigurations.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddSwagger();
builder.Services.AddControllers();
builder.Services.AddScoped<IComplaintRepository, ComplaintRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<AddComplaintCommandHandler>();
            
    cfg.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
});
        
builder.Services.AddValidatorsFromAssembly(ApplicationAssembly.Assembly, includeInternalTypes: true);
builder.Host.UseSerilog((ctx, config) =>
{
    config.ReadFrom.Configuration(ctx.Configuration);
});
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("PostgresSQL"),
        b => b.MigrationsAssembly("AdminService.DataContext"));
});

var app = builder.Build();

app.MapControllers();

app.Run();
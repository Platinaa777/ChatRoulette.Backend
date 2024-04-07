using AdminService.Api.BackgroundJob;
using AdminService.Application.Assembly;
using AdminService.Application.Behaviors;
using AdminService.Application.Commands.AddComplaint;
using AdminService.Domain.Models.ComplaintAggregate.Repos;
using AdminService.Domain.Models.FeedbackAggregate.Repos;
using AdminService.Infrastructure.Repos;
using DomainDriverDesignAbstractions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Serilog;

namespace AdminService.Api.Extensions;

public static class ServiceRegistrator
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddScoped<IComplaintRepository, ComplaintRepository>();
        builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<AddComplaintCommandHandler>();
            
            cfg.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });
        
        builder.Services.AddValidatorsFromAssembly(ApplicationAssembly.Assembly, includeInternalTypes: true);
        
        return builder;
    }

    public static WebApplicationBuilder AddDataLayer(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DataContext.Database.DataContext>(options =>
        {
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("PostgresSQL"),
                b => b.MigrationsAssembly("AdminService.DataContext"));
        });

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        using var scope = builder.Services.BuildServiceProvider().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext.Database.DataContext>();
        dbContext.Database.Migrate();

        return builder;
    }

    public static WebApplicationBuilder AddLoggingWithSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, config) =>
        {
            config.ReadFrom.Configuration(ctx.Configuration);
        });

        return builder;
    }

    public static WebApplicationBuilder AddBackgroundJobs(this WebApplicationBuilder builder)
    {
        builder.Services.AddQuartz(cfg =>
        {
            var key = new JobKey(nameof(OutboxBackgroundJob));

            cfg.AddJob<OutboxBackgroundJob>(key)
                .AddTrigger(tg => 
                    tg.ForJob(key)
                        .WithSimpleSchedule(schedule => 
                            schedule.WithIntervalInSeconds(10)
                                .RepeatForever()));
            
        });

        builder.Services.AddQuartzHostedService();

        return builder;
    }
}
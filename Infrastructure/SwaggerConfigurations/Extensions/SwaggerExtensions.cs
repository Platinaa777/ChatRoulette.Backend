using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SwaggerConfigurations.Filters;

namespace SwaggerConfigurations.Extensions;

public static class SwaggerExtensions
{
    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IStartupFilter, SwaggerFilter>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo() { 
                Title = $"{Assembly.GetEntryAssembly().GetName().Name ?? "no name"}.v1",
                Version = "v1" 
            });
            opt.CustomSchemaIds(x => x.FullName);
        });

        return builder;
    }    
}
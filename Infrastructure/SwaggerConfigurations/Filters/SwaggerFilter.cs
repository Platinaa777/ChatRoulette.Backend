using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SwaggerConfigurations.Middlewares;

namespace SwaggerConfigurations.Filters;

public class SwaggerFilter : IStartupFilter
{
    private readonly IWebHostEnvironment _env;
    
    public SwaggerFilter(IWebHostEnvironment env)
    {
        _env = env;
    }
    
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return app =>
        {
            app.UseHttpPathInfo();
            app.UseVersion();
            if (_env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            next(app);
        };
    }
}
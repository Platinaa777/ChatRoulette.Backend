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
            if (_env.IsDevelopment())
            {
                app.UseHttpPathInfo();
                app.UseVersion();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            next(app);
        };
    }
}
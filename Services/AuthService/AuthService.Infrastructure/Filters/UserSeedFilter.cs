using AuthService.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AuthService.Infrastructure.Filters;

public class UserSeedFilter : IStartupFilter
{
    private readonly IWebHostEnvironment _env;

    public UserSeedFilter(IWebHostEnvironment env)
    {
        _env = env;
    }
    
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return app =>
        {
            if (_env.IsDevelopment())
            {
                app.UseSeedUsers();
            }
            next(app);
        };
    }
}
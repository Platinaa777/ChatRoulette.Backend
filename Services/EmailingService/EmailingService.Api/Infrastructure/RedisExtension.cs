namespace EmailingService.Api.Infrastructure;

public static class RedisExtension
{
    public static WebApplicationBuilder AddCacheRedis(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services.AddStackExchangeRedisCache(setup =>
        {
            setup.Configuration = configuration["Redis:Host"];
        });

        return builder;
    }
}
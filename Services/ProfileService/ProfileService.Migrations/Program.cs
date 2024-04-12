using dotenv.net;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProfileService.Migrations;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine(Directory.GetCurrentDirectory());

        string env = "Development";
        if (args.Contains("production"))
        {
            env = "Production";
        }

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.{env}.json")
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetSection("DatabaseOptions:ConnectionString").Value;
        Console.WriteLine(connectionString);

        var services = new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(
                r =>
                {
                    r.AddPostgres()
                        .WithGlobalConnectionString(connectionString)
                        .ScanIn(typeof(Program).Assembly)
                        .For.Migrations();
                });

        var serviceProvider = services.BuildServiceProvider();

        using (var scope = serviceProvider.CreateScope())
        {
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            
            runner.MigrateUp();
        }
    }
}
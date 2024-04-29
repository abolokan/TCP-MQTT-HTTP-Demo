using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Solid.Persistence;

namespace Solid.Persitence.MigrationsHelper;

internal class Program
{
    static async Task Main(string[] args)
    {
        var hostBuilder = Host.CreateDefaultBuilder(args);

        hostBuilder.ConfigureAppConfiguration(configBuilder => configBuilder.AddJsonFile("./appsettings.json"));

        hostBuilder.ConfigureServices((context, services) =>
        {
            var connectionString = context.Configuration.GetConnectionString("DefaultConnection") ??
                throw new KeyNotFoundException("Could not find DB connection string...");
            
            services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
        });

        var host = hostBuilder.Build();
        var dbContext = host.Services.GetRequiredService<AppDbContext>();

        if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
            await dbContext.Database.MigrateAsync();
    }
}

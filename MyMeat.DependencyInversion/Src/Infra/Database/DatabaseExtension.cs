using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyMeat.Infra.Data.Contexts;

namespace MyMeat.DependencyInjection.Infra.Database;

public static class DatabaseExtension
{
    private static string GetConnectionString(IConfiguration configuration)
    {
        string? envConnectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");

        if (envConnectionString is not null)
        {
            return envConnectionString;
        }

        return configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connect string not found");
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
                  options.UseMySql(GetConnectionString(configuration),
                  new MySqlServerVersion(new Version(8, 0, 26)),
                  b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        return services;
    }

    public static IServiceCollection AddInMemoryDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
                  options.UseInMemoryDatabase("MyMeat"));

        return services;
    }
}

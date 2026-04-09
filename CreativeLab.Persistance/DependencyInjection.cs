using CreativeLab.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeLab.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration["DbConnection"];
        var serverVersion = new MySqlServerVersion(new Version(configuration["DatabaseSettings:ServerVersion"]
            ?? throw new NullReferenceException("server version was null")));

        services.AddDbContext<CreativeLabDbContext>(options =>
        {
            options.UseMySql(
                connectionString,
                serverVersion,
                mySqlOptions => mySqlOptions.EnableStringComparisonTranslations()
            );
        });

        services.AddScoped<ICreativeLabDbContext>(provider =>
            provider.GetService<CreativeLabDbContext>()
                ?? throw new NullReferenceException("provider cant't be null"));

        return services;
    }
}

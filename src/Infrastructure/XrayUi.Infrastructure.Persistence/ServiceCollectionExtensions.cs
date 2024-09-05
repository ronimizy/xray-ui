using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XrayUi.Application.Abstractions.Persistence;
using XrayUi.Application.Abstractions.Persistence.Repositories;
using XrayUi.Infrastructure.Persistence.Contexts;
using XrayUi.Infrastructure.Persistence.Options;
using XrayUi.Infrastructure.Persistence.Repositories;
using XrayUi.Infrastructure.Persistence.Services;

namespace XrayUi.Infrastructure.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection collection, IConfiguration configuration)
    {
        collection.AddDbContext<DatabaseContext>(builder => builder
            .UseSqlite($"Filename={GetDbFilePath(configuration)}")
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        collection.AddHostedService<MigrationBackgroundService>();

        collection.AddScoped<IPersistenceContext, PersistenceContext>();
        collection.AddScoped<IVpnClientRepository, VpnClientRepository>();

        return collection;
    }

    private static string GetDbFilePath(IConfiguration configuration)
    {
        var options = new PersistenceOptions();
        configuration.GetSection("Persistence").Bind(options);

        return Path.Combine(options.DatabasePath, "db.sqlite");
    }
}

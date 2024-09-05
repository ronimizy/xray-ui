using Microsoft.Extensions.DependencyInjection;
using XrayUi.Application.Clients;
using XrayUi.Application.Contracts.Clients;
using XrayUi.Application.Server;

namespace XrayUi.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddScoped<IVpnClientService, VpnClientService>();
        collection.AddSingleton<VpnServerManager>();

        collection.AddHostedService<VpnServerInitializer>();

        return collection;
    }
}

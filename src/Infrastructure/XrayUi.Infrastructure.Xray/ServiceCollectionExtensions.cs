using Microsoft.Extensions.DependencyInjection;
using XrayUi.Application.Abstractions.Xray;
using XrayUi.Infrastructure.Xray.Formatters;
using XrayUi.Infrastructure.Xray.Options;
using XrayUi.Infrastructure.Xray.Options.Client;
using XrayUi.Infrastructure.Xray.Options.Server;
using XrayUi.Infrastructure.Xray.Services;

namespace XrayUi.Infrastructure.Xray;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureXray(this IServiceCollection collection)
    {
        collection
            .AddOptions<XrayOptions>()
            .BindConfiguration("Xray")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        collection
            .AddOptions<XrayServerOptions>()
            .BindConfiguration("Xray:Server")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        collection
            .AddOptions<XrayClientOptions>()
            .BindConfiguration("Xray:Client")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        collection.AddSingleton<IXrayClientConfigFormatter, XrayClientConfigFormatter>();
        collection.AddSingleton<IXrayServerConfigFormatter, XrayServerConfigFormatter>();

        collection.AddSingleton<IXrayServerConfigManager, XrayServerConfigManager>();
        collection.AddSingleton<IXrayProcessManager, XrayProcessManager>();

        return collection;
    }
}

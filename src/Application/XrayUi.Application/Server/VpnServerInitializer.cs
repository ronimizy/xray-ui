using Microsoft.Extensions.Hosting;

namespace XrayUi.Application.Server;

internal class VpnServerInitializer : IHostedService
{
    private readonly VpnServerManager _serverManager;

    public VpnServerInitializer(VpnServerManager serverManager)
    {
        _serverManager = serverManager;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _serverManager.ReloadAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _serverManager.StopAsync(cancellationToken);
    }
}

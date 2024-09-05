using XrayUi.Application.Abstractions.Persistence;
using XrayUi.Application.Abstractions.Persistence.Queries;
using XrayUi.Application.Abstractions.Xray;
using XrayUi.Application.Models;

namespace XrayUi.Application.Server;

internal class VpnServerManager
{
    private readonly IPersistenceContext _context;
    private readonly IXrayServerConfigFormatter _configFormatter;
    private readonly IXrayServerConfigManager _configManager;
    private readonly IXrayProcessManager _processManager;

    public VpnServerManager(
        IPersistenceContext context,
        IXrayProcessManager processManager,
        IXrayServerConfigFormatter configFormatter,
        IXrayServerConfigManager configManager)
    {
        _context = context;
        _processManager = processManager;
        _configFormatter = configFormatter;
        _configManager = configManager;
    }

    public async Task ReloadAsync(CancellationToken cancellationToken)
    {
        VpnClient[] clients = await _context.VpnClients
            .QueryAsync(VpnClientQuery.Build(builder => builder), cancellationToken)
            .ToArrayAsync(cancellationToken);

        var config = new XrayServerConfig(clients);
        string formattedConfig = _configFormatter.Format(config);

        _configManager.WriteConfig(formattedConfig);

        await _processManager.StopAsync(cancellationToken);
        await _processManager.StartAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _processManager.StopAsync(cancellationToken);
    }
}

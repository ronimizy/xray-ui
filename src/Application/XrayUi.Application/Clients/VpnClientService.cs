using XrayUi.Application.Abstractions.Persistence;
using XrayUi.Application.Abstractions.Persistence.Queries;
using XrayUi.Application.Abstractions.Xray;
using XrayUi.Application.Contracts.Clients;
using XrayUi.Application.Models;
using XrayUi.Application.Server;

namespace XrayUi.Application.Clients;

internal class VpnClientService : IVpnClientService
{
    private readonly IPersistenceContext _context;
    private readonly VpnServerManager _serverManager;
    private readonly IXrayClientConfigFormatter _clientConfigFormatter;

    public VpnClientService(
        IPersistenceContext context,
        VpnServerManager serverManager,
        IXrayClientConfigFormatter clientConfigFormatter)
    {
        _context = context;
        _serverManager = serverManager;
        _clientConfigFormatter = clientConfigFormatter;
    }

    public async Task<IReadOnlyCollection<VpnClient>> ListAsync(CancellationToken cancellationToken)
    {
        return await _context.VpnClients
            .QueryAsync(VpnClientQuery.Build(builder => builder), cancellationToken)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<AddClient.Response> AddAsync(AddClient.Request request, CancellationToken cancellationToken)
    {
        var client = new VpnClient(Guid.NewGuid(), request.Name);

        await _context.VpnClients.AddAsync(client, cancellationToken);
        await _serverManager.ReloadAsync(cancellationToken);

        return new AddClient.Response(client);
    }

    public async Task<RemoveClient.Response> RemoveAsync(
        RemoveClient.Request request,
        CancellationToken cancellationToken)
    {
        VpnClient? client = await _context.VpnClients
            .QueryAsync(VpnClientQuery.Build(b => b.WithId(request.Id)), cancellationToken)
            .SingleOrDefaultAsync(cancellationToken);

        if (client is null)
            return new RemoveClient.Response.NotFound();

        await _context.VpnClients.RemoveAsync(client, cancellationToken);
        await _serverManager.ReloadAsync(cancellationToken);

        return new RemoveClient.Response.Success();
    }

    public async Task<GetClientConfig.Response> GetConfigAsync(
        GetClientConfig.Request request,
        CancellationToken cancellationToken)
    {
        VpnClient? client = await _context.VpnClients
            .QueryAsync(VpnClientQuery.Build(b => b.WithId(request.ClientId)), cancellationToken)
            .SingleOrDefaultAsync(cancellationToken);

        if (client is null)
            return new GetClientConfig.Response.NotFound();

        var clientConfig = new XrayClientConfig(client);
        string formattedConfig = _clientConfigFormatter.Format(clientConfig);

        return new GetClientConfig.Response.Success(formattedConfig);
    }
}

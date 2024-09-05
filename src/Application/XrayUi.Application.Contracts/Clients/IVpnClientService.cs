using XrayUi.Application.Models;

namespace XrayUi.Application.Contracts.Clients;

public interface IVpnClientService
{
    Task<IReadOnlyCollection<VpnClient>> ListAsync(CancellationToken cancellationToken);

    Task<AddClient.Response> AddAsync(AddClient.Request request, CancellationToken cancellationToken);

    Task<RemoveClient.Response> RemoveAsync(RemoveClient.Request request, CancellationToken cancellationToken);

    Task<GetClientConfig.Response> GetConfigAsync(GetClientConfig.Request request, CancellationToken cancellationToken);
}

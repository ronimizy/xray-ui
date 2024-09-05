using XrayUi.Application.Abstractions.Persistence.Queries;
using XrayUi.Application.Models;

namespace XrayUi.Application.Abstractions.Persistence.Repositories;

public interface IVpnClientRepository
{
    IAsyncEnumerable<VpnClient> QueryAsync(VpnClientQuery query, CancellationToken cancellationToken);

    Task AddAsync(VpnClient client, CancellationToken cancellationToken);

    Task RemoveAsync(VpnClient client, CancellationToken cancellationToken);
}

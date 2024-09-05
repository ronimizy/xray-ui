using Microsoft.EntityFrameworkCore;
using XrayUi.Application.Abstractions.Persistence.Queries;
using XrayUi.Application.Abstractions.Persistence.Repositories;
using XrayUi.Application.Models;
using XrayUi.Infrastructure.Persistence.Contexts;
using XrayUi.Infrastructure.Persistence.Models;

namespace XrayUi.Infrastructure.Persistence.Repositories;

internal class VpnClientRepository : IVpnClientRepository
{
    private readonly DatabaseContext _context;

    public VpnClientRepository(DatabaseContext context)
    {
        _context = context;
    }

    public IAsyncEnumerable<VpnClient> QueryAsync(VpnClientQuery query, CancellationToken cancellationToken)
    {
        IQueryable<VpnClientModel> queryable = _context.VpnClients;

        if (query.Ids is not [])
        {
            queryable = queryable.Where(model => query.Ids.Contains(model.Id));
        }

        return queryable
            .AsAsyncEnumerable()
            .Select(model => new VpnClient(model.Id, model.Name));
    }

    public async Task AddAsync(VpnClient client, CancellationToken cancellationToken)
    {
        var model = new VpnClientModel
        {
            Id = client.Id,
            Name = client.Name,
        };

        _context.VpnClients.Add(model);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(VpnClient client, CancellationToken cancellationToken)
    {
        await _context.VpnClients
            .Where(model => model.Id.Equals(client.Id))
            .ExecuteDeleteAsync(cancellationToken);
    }
}

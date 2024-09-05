using XrayUi.Application.Abstractions.Persistence;
using XrayUi.Application.Abstractions.Persistence.Repositories;

namespace XrayUi.Infrastructure.Persistence.Contexts;

internal class PersistenceContext : IPersistenceContext
{
    public PersistenceContext(IVpnClientRepository vpnClients)
    {
        VpnClients = vpnClients;
    }

    public IVpnClientRepository VpnClients { get; }
}

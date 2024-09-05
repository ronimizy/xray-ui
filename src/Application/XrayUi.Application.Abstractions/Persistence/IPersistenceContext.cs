using XrayUi.Application.Abstractions.Persistence.Repositories;

namespace XrayUi.Application.Abstractions.Persistence;

public interface IPersistenceContext
{
    IVpnClientRepository VpnClients { get; }
}

namespace XrayUi.Application.Models;

public readonly record struct XrayServerConfig(IReadOnlyCollection<VpnClient> Clients);

using XrayUi.Application.Models;

namespace XrayUi.Application.Contracts.Clients;

public static class AddClient
{
    public readonly record struct Request(string Name);

    public readonly record struct Response(VpnClient Client);
}

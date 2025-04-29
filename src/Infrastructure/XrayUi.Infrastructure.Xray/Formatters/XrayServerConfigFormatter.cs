using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using XrayUi.Application.Abstractions.Xray;
using XrayUi.Application.Models;
using XrayUi.Infrastructure.Xray.Options.Server;

namespace XrayUi.Infrastructure.Xray.Formatters;

internal class XrayServerConfigFormatter : IXrayServerConfigFormatter
{
    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        Formatting = Formatting.Indented,
    };

    private readonly IOptionsMonitor<XrayServerOptions> _options;

    public XrayServerConfigFormatter(IOptionsMonitor<XrayServerOptions> options)
    {
        _options = options;
    }

    public string Format(XrayServerConfig config)
    {
        object configObject = CreateConfigObject(config);
        return JsonConvert.SerializeObject(configObject, SerializerSettings);
    }

    private object CreateConfigObject(XrayServerConfig config)
    {
        XrayServerOptions options = _options.CurrentValue;

        return new
        {
            log = new
            {
                loglevel = options.LogLevel,
            },
            inbounds = new object[]
            {
                new
                {
                    tag = "dokodemo-in",
                    port = options.InboundPort,
                    protocol = "dokodemo-door",
                    settings = new
                    {
                        address = "127.0.0.1",
                        port = 4431,
                        network = "tcp",
                    },
                    sniffing = new
                    {
                        enabled = true,
                        destOverride = new object[]
                        {
                            "tls",
                        },
                        routeOnly = true,
                    },
                },
                new
                {
                    listen = "127.0.0.1",
                    port = 4431,
                    protocol = "vless",
                    settings = new
                    {
                        clients = config.Clients.Select(CreateClientObject).ToArray(),
                        decryption = "none",
                    },
                    streamSettings = new
                    {
                        network = "tcp",
                        security = "reality",
                        realitySettings = new
                        {
                            dest = options.Reality.Destination,
                            serverNames = options.Reality.ServerNames,
                            privateKey = options.PrivateKey,
                            shortIds = new[]
                            {
                                string.Empty,
                                "0123456789abcdef",
                            },
                        },
                    },
                    sniffing = new
                    {
                        enabled = true,
                        destOverride = new[]
                        {
                            "http",
                            "tls",
                            "quic",
                        },
                        routeOnly = true,
                    },
                },
            },
            outbounds = new object[]
            {
                new
                {
                    protocol = "freedom",
                    tag = "direct",
                },
                new
                {
                    protocol = "blackhole",
                    tag = "block",
                },
                new
                {
                    protocol = "dns",
                    tag = "dns-out",
                },
            },
            routing = new
            {
                rules = new object[]
                {
                    new
                    {
                        type = "field",
                        network = "udp",
                        port = 53,
                        outboundTag = "dns-out",
                    },
                    new
                    {
                        inboundTag = new[] { "dokodemo-in" },
                        domain = options.Reality.ServerNames,
                        outboundTag = "direct",
                    },
                    new
                    {
                        inboundTag = new[] { "dokodemo-in" },
                        outboundTag = "block",
                    },
                },
            },
            dns = new
            {
                servers = options.Dns.Servers.Select(server => new
                {
                    address = server.Address,
                    port = server.Port,
                }),
            },
        };
    }

    private object CreateClientObject(VpnClient client)
    {
        return new
        {
            id = client.Id.ToString(),
        };
    }
}

using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using XrayUi.Application.Abstractions.Xray;
using XrayUi.Application.Models;
using XrayUi.Infrastructure.Xray.Options.Client;
using XrayUi.Infrastructure.Xray.Options.Server;

namespace XrayUi.Infrastructure.Xray.Formatters;

internal class XrayClientConfigFormatter : IXrayClientConfigFormatter
{
    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        Formatting = Formatting.Indented,
    };

    private readonly IOptionsMonitor<XrayClientOptions> _clientOptions;
    private readonly IOptionsMonitor<XrayServerOptions> _serverOptions;

    public XrayClientConfigFormatter(
        IOptionsMonitor<XrayClientOptions> clientOptions,
        IOptionsMonitor<XrayServerOptions> serverOptions)
    {
        _clientOptions = clientOptions;
        _serverOptions = serverOptions;
    }

    public string Format(XrayClientConfig config)
    {
        object configObject = CreateConfigObject(config);
        return JsonConvert.SerializeObject(configObject, SerializerSettings);
    }

    private object CreateConfigObject(XrayClientConfig config)
    {
        XrayClientOptions clientOptions = _clientOptions.CurrentValue;
        XrayServerOptions serverOptions = _serverOptions.CurrentValue;

        return new
        {
            log = new
            {
                loglevel = clientOptions.LogLevel,
            },
            outbounds = new object[]
            {
                new
                {
                    protocol = "vless",
                    settings = new
                    {
                        vnext = new object[]
                        {
                            new
                            {
                                address = clientOptions.ServerAddress,
                                port = clientOptions.ServerPort,
                                users = new object[]
                                {
                                    new
                                    {
                                        id = config.Client.Id.ToString(),
                                        encryption = "none",
                                    },
                                },
                            },
                        },
                    },
                    streamSettings = new
                    {
                        network = "tcp",
                        security = "reality",
                        realitySettings = new
                        {
                            fingerprint = "chrome",
                            serverName = serverOptions.Reality.ServerNames.FirstOrDefault() ?? string.Empty,
                            publicKey = clientOptions.PublicKey,
                            spiderX = string.Empty,
                            shortId = string.Empty,
                        },
                    },
                    tag = "proxy",
                },
            },
        };
    }
}

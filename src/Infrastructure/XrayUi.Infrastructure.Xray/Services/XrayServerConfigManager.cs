using Microsoft.Extensions.Options;
using XrayUi.Application.Abstractions.Xray;
using XrayUi.Infrastructure.Xray.Options;

namespace XrayUi.Infrastructure.Xray.Services;

internal class XrayServerConfigManager : IXrayServerConfigManager
{
    private readonly IOptionsMonitor<XrayOptions> _options;

    public XrayServerConfigManager(IOptionsMonitor<XrayOptions> options)
    {
        _options = options;
    }

    public void WriteConfig(string config)
    {
        string path = _options.CurrentValue.ConfigPath;
        File.WriteAllText(path, config);
    }
}

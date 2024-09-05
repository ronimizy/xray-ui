namespace XrayUi.Infrastructure.Xray.Options.Server;

public class XrayServerOptions
{
    public string LogLevel { get; set; } = string.Empty;

    public int InboundPort { get; set; }

    public string PrivateKey { get; set; } = string.Empty;

    public XrayRealityOptions Reality { get; set; } = new();
}

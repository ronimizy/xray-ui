namespace XrayUi.Infrastructure.Xray.Options.Client;

public class XrayClientOptions
{
    public string LogLevel { get; set; } = string.Empty;

    public string ServerAddress { get; set; } = string.Empty;

    public int ServerPort { get; set; }

    public string PublicKey { get; set; } = string.Empty;
}

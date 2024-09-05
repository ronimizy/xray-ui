namespace XrayUi.Application.Abstractions.Xray;

public interface IXrayProcessManager
{
    Task StartAsync(CancellationToken cancellationToken);

    Task StopAsync(CancellationToken cancellationToken);
}

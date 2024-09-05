using CliWrap;
using Itmo.Dev.Platform.Common.Extensions;
using Itmo.Dev.Platform.Common.Tools;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XrayUi.Application.Abstractions.Xray;
using XrayUi.Infrastructure.Xray.Options;

namespace XrayUi.Infrastructure.Xray.Services;

internal class XrayProcessManager : IXrayProcessManager, IDisposable
{
    private readonly IOptionsMonitor<XrayOptions> _options;
    private readonly SemaphoreSlim _semaphore;
    private readonly ILogger<XrayProcessManager> _logger;

    private Task? _task;
    private CancellationTokenSource? _cts;

    public XrayProcessManager(IOptionsMonitor<XrayOptions> options, ILogger<XrayProcessManager> logger)
    {
        _options = options;
        _logger = logger;
        _semaphore = new SemaphoreSlim(1, 1);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using SemaphoreSubscription ss = await _semaphore.UseAsync(cancellationToken);

        if (_cts is not null)
            throw new InvalidOperationException("Trying to start process when process already running");

        string configPath = _options.CurrentValue.ConfigPath;
        _cts = new CancellationTokenSource();

        _task = Cli
            .Wrap("xray")
            .WithStandardInputPipe(PipeSource.FromFile(configPath))
            .WithStandardOutputPipe(PipeTarget.ToDelegate(log => _logger.LogInformation("Xray: {Log}", log)))
            .WithStandardErrorPipe(PipeTarget.ToDelegate(log => _logger.LogWarning("Xray: {Log}", log)))
            .WithValidation(CommandResultValidation.None)
            .ExecuteAsync(_cts.Token);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        using SemaphoreSubscription ss = await _semaphore.UseAsync(cancellationToken);

        if (_cts is null || _task is null)
        {
            _logger.LogWarning("Trying to stop non existent process");
            return;
        }

        await _cts.CancelAsync();
        _cts = null;

        try
        {
            await _task;
        }
        catch (Exception e) when (e is OperationCanceledException)
        {
            // Ignore cancellation exceptions
        }
    }

    public void Dispose()
    {
        _semaphore.Dispose();
        _cts?.Dispose();
    }
}

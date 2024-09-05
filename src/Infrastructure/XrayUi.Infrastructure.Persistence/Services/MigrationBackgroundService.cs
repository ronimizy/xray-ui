using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XrayUi.Infrastructure.Persistence.Contexts;

namespace XrayUi.Infrastructure.Persistence.Services;

internal class MigrationBackgroundService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public MigrationBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using AsyncServiceScope scope = _scopeFactory.CreateAsyncScope();
        DatabaseContext context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        await context.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

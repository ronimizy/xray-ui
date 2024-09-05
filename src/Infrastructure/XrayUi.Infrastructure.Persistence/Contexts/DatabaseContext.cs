using Microsoft.EntityFrameworkCore;
using XrayUi.Infrastructure.Persistence.Models;

namespace XrayUi.Infrastructure.Persistence.Contexts;

internal class DatabaseContext : DbContext
{
    public DbSet<VpnClientModel> VpnClients => Set<VpnClientModel>();

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
}

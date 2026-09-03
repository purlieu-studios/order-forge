public sealed class InventoryDbContext(DbContextOptions<InventoryDbContext> options)
      : DbContext(options)
{
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
}

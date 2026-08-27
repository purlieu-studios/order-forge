public sealed class OrdersDbContext(DbContextOptions<OrdersDbContext> options)
      : DbContext(options)
{
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Order> Orders => Set<Order>();
}
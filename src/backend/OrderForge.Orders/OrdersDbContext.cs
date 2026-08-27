using Microsoft.EntityFrameworkCore;

public sealed class OrdersDbContext(DbContextOptions<OrdersDbContext> options)
      : DbContext(options)
{
}
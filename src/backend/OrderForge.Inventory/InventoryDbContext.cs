using Microsoft.EntityFrameworkCore;

public sealed class InventoryDbContext(DbContextOptions<InventoryDbContext> options)
      : DbContext(options)
{
}
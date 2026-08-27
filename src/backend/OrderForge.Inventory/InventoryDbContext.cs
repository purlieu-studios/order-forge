using Microsoft.EntityFrameworkCore;

namespace OrderForge;

public sealed class InventoryDbContext(DbContextOptions<InventoryDbContext> options)
      : DbContext(options)
{
}
using Microsoft.EntityFrameworkCore;

namespace OrderForge;

public sealed class OrdersDbContext(DbContextOptions<OrdersDbContext> options)
      : DbContext(options)
{
}
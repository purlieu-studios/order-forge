using Microsoft.EntityFrameworkCore;

namespace OrderForge;

public sealed class PaymentsDbContext(DbContextOptions<PaymentsDbContext> options)
      : DbContext(options)
{
}
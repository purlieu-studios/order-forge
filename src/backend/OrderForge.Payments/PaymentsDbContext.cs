using Microsoft.EntityFrameworkCore;

public sealed class PaymentsDbContext(DbContextOptions<PaymentsDbContext> options)
      : DbContext(options)
{
}
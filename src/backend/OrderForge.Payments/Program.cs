var builder = WebApplication.CreateBuilder(args);

var paymentsDatabase = builder.Configuration.GetConnectionString("PaymentsDatabase")
    ?? throw new InvalidOperationException(
        "Connection string 'PaymentsDatabase' is not configured.");

builder.Services.AddDbContextFactory<PaymentsDbContext>(options =>
      options.UseNpgsql(paymentsDatabase));

var app = builder.Build();

app.Run();
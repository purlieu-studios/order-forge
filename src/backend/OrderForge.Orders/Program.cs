var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidation();
var ordersDatabase = builder.Configuration.GetConnectionString("OrdersDatabase")
    ?? throw new InvalidOperationException(
        "Connection string 'OrdersDatabase' is not configured.");

builder.Services.AddDbContextFactory<OrdersDbContext>(options =>
      options.UseNpgsql(ordersDatabase));

var app = builder.Build();

app.Run();
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidation();
var inventoryDatabase = builder.Configuration.GetConnectionString("InventoryDatabase")
    ?? throw new InvalidOperationException(
        "Connection string 'InventoryDatabase' is not configured.");

builder.Services.AddDbContextFactory<InventoryDbContext>(options =>
      options.UseNpgsql(inventoryDatabase));

var app = builder.Build();

app.Run();
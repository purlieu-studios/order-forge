using Microsoft.EntityFrameworkCore;
using OrderForge;

var builder = WebApplication.CreateBuilder(args);

var inventoryDatabase = builder.Configuration.GetConnectionString("InventoryDatabase")
    ?? throw new InvalidOperationException(
        "Connection string 'InventoryDatabase' is not configured.");

builder.Services.AddDbContextFactory<InventoryDbContext>(options =>
      options.UseNpgsql(inventoryDatabase));

var app = builder.Build();

app.Run();
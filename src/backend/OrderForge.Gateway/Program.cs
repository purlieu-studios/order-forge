var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var ordersBaseUrl = builder.Configuration["Services:Orders:BaseUrl"]
     ?? throw new InvalidOperationException(
         "Configuration 'Services:Orders:BaseUrl' is not configured.");

builder.Services.AddHttpClient("Orders", client => client.BaseAddress = new Uri(ordersBaseUrl));

var inventoryBaseUrl = builder.Configuration["Services:Inventory:BaseUrl"]
    ?? throw new InvalidOperationException(
        "Configuration 'Services:Inventory:BaseUrl' is not configured.");

builder.Services.AddHttpClient("Inventory", client => client.BaseAddress = new Uri(inventoryBaseUrl));

var app = builder.Build();
app.MapControllers();

app.Run();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var ordersBaseUrl = builder.Configuration["Services:Orders:BaseUrl"]
     ?? throw new InvalidOperationException(
         "Configuration 'Services:Orders:BaseUrl' is not configured.");

builder.Services.AddHttpClient("Orders", client => client.BaseAddress = new Uri(ordersBaseUrl));

var inventoryBaseUrl = builder.Configuration["Services:Inventory:BaseUrl"]
    ?? throw new InvalidOperationException(
        "Configuration 'Services:Inventory:BaseUrl' is not configured.");

builder.Services.AddHttpClient("Inventory", client => client.BaseAddress = new Uri(inventoryBaseUrl));

var app = builder.Build();
app.UseCors("Frontend");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();

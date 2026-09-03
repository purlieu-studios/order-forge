var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var ordersBaseUrl = builder.Configuration["Services:Orders:BaseUrl"]
     ?? throw new InvalidOperationException(
         "Configuration 'Services:Orders:BaseUrl' is not configured.");

builder.Services.AddHttpClient("Orders", client => client.BaseAddress = new Uri(ordersBaseUrl));


var app = builder.Build();
app.MapControllers();

app.Run();

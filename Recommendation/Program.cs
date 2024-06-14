using System.Globalization;
using Recommendation.Infra;
using Recommendation.Services;

var builder = WebApplication.CreateBuilder(args);

// define en-US como padrao
var culture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

NativeInjector.RegisterServices(builder.Services, builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Recommendation API is running");

app.MapGet("/saveAllModels", async () =>
{
    using var scope = app.Services.CreateScope();
    var tickerTreinamentoService = scope.ServiceProvider.GetRequiredService<TickerTreinamentoService>();

    await tickerTreinamentoService.SaveBuyModels();
    await tickerTreinamentoService.SaveLowModels();
    await tickerTreinamentoService.SaveSellModels();
    await tickerTreinamentoService.SaveHighModels();

    return "AllModels saved!";
});

# region [ Compra - Sugestoes - V4 ]
app.MapGet("/getBuyValue", (string symbol) =>
{
    if (!symbol.Contains("-BRL", StringComparison.CurrentCultureIgnoreCase))
    {
        symbol = $"{symbol.ToUpper()}-BRL";
    }
    using var scope = app.Services.CreateScope();
    var tickerService = scope.ServiceProvider.GetRequiredService<TickerSugestaoService>();

    var value = decimal.Parse(tickerService.GetBuyValue(symbol.ToUpper()).ToString("N8"));
    Console.WriteLine($"symbol: {symbol.ToUpper()} - Type: Buy => {value}");
    return value;
});

app.MapGet("/getLowValue", (string symbol) =>
{
    if (!symbol.Contains("-BRL", StringComparison.CurrentCultureIgnoreCase))
    {
        symbol = $"{symbol.ToUpper()}-BRL";
    }
    using var scope = app.Services.CreateScope();
    var tickerService = scope.ServiceProvider.GetRequiredService<TickerSugestaoService>();

    var value = decimal.Parse(tickerService.GetLowValue(symbol.ToUpper()).ToString("N8"));
    Console.WriteLine($"symbol: {symbol.ToUpper()} - Type: Low => {value}");
    return value;
});

app.MapGet("/getSellValue", (string symbol) =>
{
    if (!symbol.Contains("-BRL", StringComparison.CurrentCultureIgnoreCase))
    {
        symbol = $"{symbol.ToUpper()}-BRL";
    }
    using var scope = app.Services.CreateScope();
    var tickerService = scope.ServiceProvider.GetRequiredService<TickerSugestaoService>();

    var value = decimal.Parse(tickerService.GetSellValue(symbol.ToUpper()).ToString("N8"));
    Console.WriteLine($"symbol: {symbol.ToUpper()} - Type: Sell => {value}");
    return value;
});

app.MapGet("/getHighValue", (string symbol) =>
{
    if (!symbol.Contains("-BRL", StringComparison.CurrentCultureIgnoreCase))
    {
        symbol = $"{symbol.ToUpper()}-BRL";
    }
    using var scope = app.Services.CreateScope();
    var tickerService = scope.ServiceProvider.GetRequiredService<TickerSugestaoService>();

    var value = decimal.Parse(tickerService.GetHighValue(symbol.ToUpper()).ToString("N8"));
    Console.WriteLine($"symbol: {symbol.ToUpper()} - Type: High => {value}");
    return value;
});

# endregion

app.Run();
using System.Globalization;
using DataCollection.Infra;
using DataCollection.Services;

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

app.MapGet("/", () => "DataCollection API is running");

app.MapGet("/saveSymbols", () =>
{
    using var scope = app.Services.CreateScope();
    var symbolService = scope.ServiceProvider.GetRequiredService<SymbolService>();
    var result = symbolService.SaveSymbolsAsync().Result;

    return result switch
    {
        1 => Results.Ok($"Saved {result} new symbol"),
        > 1 => Results.Ok($"Saved {result} new symbols"),
        _ => Results.Ok("No new symbols to save")
    };
});

app.MapGet("/saveTickers", () =>
{
    using var scope = app.Services.CreateScope();
    var dataCollectionService = scope.ServiceProvider.GetRequiredService<TickerService>();
    var result = dataCollectionService.CollectAndSaveTickersAsync().Result;

    return result switch
    {
        1 => Results.Ok($"Saved {result} ticker"),
        > 1 => Results.Ok($"Saved {result} tickers"),
        _ => Results.Ok("No tickers to save")
    };
});

app.Run();
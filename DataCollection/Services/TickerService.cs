using System.Text.Json;
using DataCollection.Data.PostgreSQL;
using DataCollection.Entities;

namespace DataCollection.Services
{
    public class TickerService(IConfiguration configuration, MongoDbService mongoDbService, HttpClient httpClient, DataCollectionDbContext context, ILogger<TickerService> logger)
    {
        private readonly MongoDbService _mongoDbService = mongoDbService;
        private readonly HttpClient _httpClient = httpClient;
        private readonly DataCollectionDbContext _context = context;
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<TickerService> _logger = logger;

        public async Task<int> CollectAndSaveTickersAsync()
        {
            try
            {
                var listSymbols = _context.Symbols.Select(s => s.SymbolName).ToList();
                if (listSymbols.Count == 0)
                {
                    _logger.LogWarning("No symbols found in the database.");
                    return 0;
                }

                var symbols = string.Join(",", listSymbols);
                var response = await _httpClient.GetAsync($"{_configuration["ApiUrlMercadobitcoinV4"]}tickers?symbols={symbols}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var tickersRequest = JsonSerializer.Deserialize<List<TickerRequest>>(content);

                    if (tickersRequest != null)
                    {
                        var tickers = tickersRequest.Select(tickerRequest => new Ticker(tickerRequest)).ToList();
                        await _mongoDbService.SaveTickersAsync(tickers);
                        _logger.LogInformation("Tickers successfully saved to MongoDB.");
                        return tickers.Count;
                    }
                    else
                    {
                        _logger.LogWarning("No tickers found in the response.");
                        return 0;
                    }
                }
                else
                {
                    _logger.LogError($"{response.StatusCode} => Error fetching tickers");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while collecting and saving tickers.");
                return 0;
            }
        }
    }
}

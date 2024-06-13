using System.Text.Json;
using DataCollection.Data.PostgreSQL;
using DataCollection.Entities;

namespace DataCollection.Services
{
    public class TickerService
    {
        private readonly MongoDbService _mongoDbService;
        private readonly HttpClient _httpClient;
        private readonly DataCollectionDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TickerService> _logger;

        public TickerService(IConfiguration configuration, MongoDbService mongoDbService, HttpClient httpClient, DataCollectionDbContext context, ILogger<TickerService> logger)
        {
            _configuration = configuration;
            _mongoDbService = mongoDbService;
            _httpClient = httpClient;
            _context = context;
            _logger = logger;
        }

        public async Task<int> CollectAndSaveTickersAsync()
        {
            try
            {
                var tickers = await CollectTickersAsync();
                if (tickers == null || tickers.Count == 0)
                {
                    _logger.LogWarning("No tickers to save.");
                    return 0;
                }

                await SaveTickersAsync(tickers);
                return tickers.Count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while collecting and saving tickers.");
                return 0;
            }
        }

        private async Task<List<Ticker>> CollectTickersAsync()
        {
            var listSymbols = _context.Symbols.Select(s => s.SymbolName).ToList();
            if (listSymbols.Count == 0)
            {
                _logger.LogWarning("No symbols found in the database.");
                return null;
            }

            var symbols = string.Join(",", listSymbols);
            Console.WriteLine($"{_configuration["ApiUrlMercadobitcoinV4"]}tickers?symbols={symbols}");
            var response = await _httpClient.GetAsync($"{_configuration["ApiUrlMercadobitcoinV4"]}tickers?symbols={symbols}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"{response.StatusCode} => Error fetching tickers");
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var tickersRequest = JsonSerializer.Deserialize<List<TickerRequest>>(content);

            if (tickersRequest != null)
                return tickersRequest.Select(tickerRequest => new Ticker(tickerRequest)).ToList();
            _logger.LogWarning("No tickers found in the response.");
            return null;

        }

        private async Task SaveTickersAsync(List<Ticker> tickers)
        {
            await _mongoDbService.SaveTickersAsync(tickers);
            _logger.LogInformation("Tickers successfully saved to MongoDB.");
        }
    }
}

using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Recommendation.Data.MongoDB;
using Recommendation.Entities;

namespace Recommendation.Services
{
    public class MongoDbService
    {
        private readonly IMongoCollection<Ticker> _tickersCollection;
        private readonly ILogger<MongoDbService> _logger;

        public MongoDbService(IOptions<MongoDbSettings> mongoDbSettings, ILogger<MongoDbService> logger)
        {
            _logger = logger;
            var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _tickersCollection = mongoDatabase.GetCollection<Ticker>(mongoDbSettings.Value.TickersCollectionName);
            CreateIndexes().GetAwaiter().GetResult();
        }

        private async Task CreateIndexes()
        {
            try
            {
                var indexKeys = Builders<Ticker>.IndexKeys.Ascending(t => t.Pair).Descending(t => t.Id);
                var indexOptions = new CreateIndexOptions { Background = true };
                var indexModel = new CreateIndexModel<Ticker>(indexKeys, indexOptions);
                await _tickersCollection.Indexes.CreateOneAsync(indexModel);
                _logger.LogInformation("Indexes created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating indexes: {ex.Message}");
            }
        }

        public async Task<List<Ticker>> GetRecentTickersAsync(string symbol, int count = 1)
        {
            try
            {
                var filter = Builders<Ticker>.Filter.Eq(t => t.Pair, symbol);
                var sort = Builders<Ticker>.Sort.Descending(t => t.Id);

                return await _tickersCollection.Find(filter)
                                               .Sort(sort)
                                               .Limit(count)
                                               .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching recent tickers: {ex.Message}");
                return [];
            }
        }
    }
}

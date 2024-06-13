using DataCollection.Data.MongoDB;
using DataCollection.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DataCollection.Services
{
    public class MongoDbService
    {
        private readonly IMongoCollection<Ticker> _tickersCollection;

        public MongoDbService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _tickersCollection = mongoDatabase.GetCollection<Ticker>(mongoDbSettings.Value.TickersCollectionName);
        }

        public async Task SaveTickersAsync(IEnumerable<Ticker> tickers)
        {
            await _tickersCollection.InsertManyAsync(tickers);
        }

        public async Task DeleteManyAsync(FilterDefinition<Ticker> filter)
        {
            await _tickersCollection.DeleteManyAsync(filter);
        }

        public async Task<List<Ticker>> FindAsync(FilterDefinition<Ticker> filter)
        {
            return await _tickersCollection.Find(filter).ToListAsync();
        }
    }
}
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

        public async Task SaveTickersAsync(IEnumerable<Ticker> ticker)
        {
            await _tickersCollection.InsertManyAsync(ticker);
        }
    }
}
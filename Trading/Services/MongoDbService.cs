using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Trading.Data.MongoDB;

namespace Trading.Services;

public class MongoDbService
{
    public MongoDbService(IOptions<MongoDbSettings> mongoDbSettings)
    {
        var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
    }
}
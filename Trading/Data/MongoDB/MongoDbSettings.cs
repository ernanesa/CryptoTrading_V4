namespace Trading.Data.MongoDB;

public class MongoDbSettings : IMongoDbSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string TickersCollectionName { get; set; }
}

public interface IMongoDbSettings
{
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
    string TickersCollectionName { get; set; }
}
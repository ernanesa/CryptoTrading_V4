using DataCollection.Data.MongoDB;
using DataCollection.Data.PostgreSQL;
using DataCollection.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DataCollection.Infra
{
    public class NativeInjector
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataCollectionDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("CryptoTradingDB")));

            #region MongoDB configuration

            services.Configure<MongoDbSettings>(
                configuration.GetSection(nameof(MongoDbSettings)));

            services.AddSingleton<IMongoDbSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

            services.AddSingleton<IMongoClient>(s =>
            {
                var settings = s.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                return new MongoClient(settings.ConnectionString);
            });

            services.AddSingleton<IMongoDatabase>(s =>
            {
                var settings = s.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                var client = s.GetRequiredService<IMongoClient>();
                return client.GetDatabase(settings.DatabaseName);
            });
            #endregion

            services.AddHttpClient();

            services.AddScoped<SymbolService>();
            services.AddSingleton<MongoDbService>();
            services.AddScoped<TickerService>();
        }
    }
}
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Trading.Data.MongoDB;
using Trading.Data.PostgreSQL;
using Trading.Repositories;
using Trading.Services;

namespace Trading.Infra;

public class NativeInjector
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TradingDbContext>(options =>
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


        #region Services
        services.AddSingleton<MongoDbService>();
        // services.AddScoped<SystemConfigurationService>();
        // services.AddScoped<TickerService>();
        services.AddScoped<UserService>();
        services.AddScoped<AuthenticationService>();
        // services.AddScoped<AccountService>();
        #endregion

        #region Repositories
        services.AddScoped<UserRepository>();
        // services.AddScoped<SystemConfigurationRepository>();
        // services.AddScoped<TickerRepository>();
        // services.AddScoped<OrderRepository>();
        // services.AddScoped<AccountRepository>();
        #endregion

        // Configurar HttpClient
        services.AddHttpClient<Services.AuthenticationService>(client =>
        {
            client.BaseAddress = new Uri(configuration["ApiUrlMercadobitcoinV4"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });
    }
}
using DataCollection.Data.PostgreSQL;
using DataCollection.Services;
using Microsoft.EntityFrameworkCore;

namespace DataCollection.Infra
{
    public class NativeInjector
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataCollectionDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("CryptoTradingDB")));

            services.AddHttpClient();

            services.AddScoped<SymbolService>();
        }
    }
}
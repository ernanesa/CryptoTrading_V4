using Microsoft.EntityFrameworkCore;
using Scheduler.Data;

namespace Scheduler.Infra
{
    public class NativeInjector
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SchedulerDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("CryptoTradingDB")));
        }
    }
}
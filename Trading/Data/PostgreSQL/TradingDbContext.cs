using Microsoft.EntityFrameworkCore;

namespace Trading.Data.PostgreSQL;

public class TradingDbContext(DbContextOptions<TradingDbContext> options) : DbContext(options)
{
}
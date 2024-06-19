using Microsoft.EntityFrameworkCore;
using Trading.Entities;

namespace Trading.Data.PostgreSQL;

public class TradingDbContext(DbContextOptions<TradingDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}
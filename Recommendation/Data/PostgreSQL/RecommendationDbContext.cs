using Microsoft.EntityFrameworkCore;
using Recommendation.Entities;

namespace Recommendation.Data.PostgreSQL;

public class RecommendationDbContext(DbContextOptions<RecommendationDbContext> options) : DbContext(options)
{
    public DbSet<Symbol> Symbols { get; set; }
}
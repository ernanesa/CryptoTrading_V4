using DataCollection.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataCollection.Data.PostgreSQL
{
    public class DataCollectionDbContext(DbContextOptions<DataCollectionDbContext> options) : DbContext(options)
    {
        public DbSet<Symbol> Symbols { get; set; }
    }
}
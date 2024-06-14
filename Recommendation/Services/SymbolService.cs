using Recommendation.Data.PostgreSQL;
using Recommendation.Entities;

namespace Recommendation.Services
{
    public class SymbolService
    {
        private readonly RecommendationDbContext _context;

        public SymbolService(RecommendationDbContext context)
        {
            _context = context;
        }

        public List<Symbol> GetSymbols()
        {
            try
            {
                return [.. _context.Symbols];
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error fetching symbols from database: {e.Message}");
                return [];
            }
        }
    }
}
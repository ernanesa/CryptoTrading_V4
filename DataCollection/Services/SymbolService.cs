using System.Text.Json;
using DataCollection.Data.PostgreSQL;
using DataCollection.Entities;

namespace DataCollection.Services
{
    public class SymbolService(IConfiguration configuration, DataCollectionDbContext context, HttpClient httpClient)
    {
        public async Task<int> SaveSymbolsAsync()
        {
            var symbolsFromApi = await FetchSymbolsFromApiAsync();
            var symbolsFromDatabase = FetchSymbolsFromDatabase();

            var newSymbols = symbolsFromApi.Where(apiSymbol => symbolsFromDatabase.All(dbSymbol => dbSymbol.SymbolName != apiSymbol.SymbolName)).ToList();

            if (newSymbols.Count != 0)
            {
                context.Symbols.AddRange(newSymbols);
                await context.SaveChangesAsync();
            }

            return newSymbols.Count;
        }

        private async Task<List<Symbol>> FetchSymbolsFromApiAsync()
        {
            var symbols = new List<Symbol>();
            var response = await httpClient.GetAsync($"{configuration["ApiUrlMercadobitcoinV4"]}symbols");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"{response.StatusCode} => Error fetching symbols");
                return symbols;
            }

            var content = await response.Content.ReadAsStringAsync();
            var symbolRequest = JsonSerializer.Deserialize<SymbolRequest>(content);

            if (symbolRequest?.Symbol == null)
            {
                return symbols;
            }

            for (var i = 0; i < symbolRequest.Symbol.Count; i++)
            {
                try
                {
                    var symbol = MapToSymbol(symbolRequest, i);

                    if (IsValidSymbol(symbol))
                    {
                        symbols.Add(symbol);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error processing symbol: {e.Message}");
                }
            }

            return [.. symbols.OrderBy(s => s.SymbolName)];
        }

        private static double ParseDouble(string value)
        {
            return double.TryParse(value, out var result) ? result : 0;
        }

        private static bool IsValidSymbol(Symbol symbol)
        {
            var validTypes = new[] { "CRYPTO", "DEFI", "UTILITY_TOKEN" }; // TODO: Get parameters from database

            return symbol.SymbolName != null && validTypes.Contains(symbol.Type?.ToUpper()) && symbol.SymbolName.Contains("-BRL");
        }

        private List<Symbol> FetchSymbolsFromDatabase()
        {
            try
            {
                return [.. context.Symbols];
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error fetching symbols from database: {e.Message}");
                return [];
            }
        }

        private static Symbol MapToSymbol(SymbolRequest symbols, int index)
        {
            return new Symbol
            {
                BaseCurrency = symbols.BaseCurrency[index],
                Currency = symbols.Currency[index],
                DepositMinimum = ParseDouble(symbols.DepositMinimum[index]),
                Description = symbols.Description[index],
                ExchangeListed = symbols.ExchangeListed[index],
                ExchangeTraded = symbols.ExchangeTraded[index],
                MinMovement = ParseDouble(symbols.MinMovement[index]),
                PriceScale = symbols.PriceScale[index],
                SessionRegular = symbols.SessionRegular[index],
                SymbolName = symbols.Symbol[index],
                Timezone = symbols.Timezone[index],
                Type = symbols.Type[index],
                WithdrawMinimum = ParseDouble(symbols.WithdrawMinimum[index]),
                WithdrawalFee = ParseDouble(symbols.WithdrawalFee[index]),
                IsActive = true
            };
        }
    }
}
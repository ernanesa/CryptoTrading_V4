using System.Text.Json;
using Trading.Entities;

namespace Trading.Repositories
{
    public class AccountRepository(HttpClient httpClient, IConfiguration configuration) : Repository(httpClient, configuration)
    {
        public async Task<Account> GetMainAccountAsync(User user)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.AccessToken);
            var response = await _httpClient.GetAsync($"accounts");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var accounts = JsonSerializer.Deserialize<List<Account>>(content);

                return accounts.FirstOrDefault(a => a.Name.ToLower() == "main");
            }

            throw new Exception($"Failed to fetch account. Status code: {response.StatusCode}");
        }

        public async Task<List<Balance>> GetAccountBalancesAsync(User user)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.AccessToken);
            var account = await GetMainAccountAsync(user);
            var response = await _httpClient.GetAsync($"accounts/{account.Id}/balances");

            if (response.IsSuccessStatusCode)
            {
                // return only currencies with a positive balance
                var content = await response.Content.ReadAsStringAsync();
                var balances = JsonSerializer.Deserialize<List<Balance>>(content);
                return balances.Where(b => b.Total != "0.00000000").ToList();
            }

            throw new Exception($"Failed to fetch balances. Status code: {response.StatusCode}");
        }

        public async Task<decimal> GetTotalBalanceAsync(User user)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.AccessToken);
            var account = await GetMainAccountAsync(user);
            var balanceResponse = await _httpClient.GetAsync($"accounts/{account.Id}/balances");
            
            if (balanceResponse.IsSuccessStatusCode)
            {
                // return only currencies with a positive balance
                var content = await balanceResponse.Content.ReadAsStringAsync();
                var balances = JsonSerializer.Deserialize<List<Balance>>(content);
                var positiveBalances= balances.Where(b => b.Total != "0.00000000").ToList();
                var listSymbols  = positiveBalances.Select(b => b.Symbol).ToList();
                var tickersRequest = CollectTickersAsync(listSymbols).Result;

                var total = positiveBalances.Sum(b =>
                {
                    var ticker = tickersRequest.FirstOrDefault(t => t.Pair == b.Symbol+"-BRL");
                    return decimal.Parse(b.Total) * decimal.Parse(ticker.Last);
                });

                // retornar o total com 2 casas decimais
                return Math.Round(total, 2);
            }

            throw new Exception($"Failed to fetch balances. Status code: {balanceResponse.StatusCode}");
        }

        private async Task<List<TickerRequest>> CollectTickersAsync(List<string> listSymbols)
        {
            try{
                if (listSymbols.Count == 0)
                {
                    return null;
                }

                // se não tiverem o -BRL, adicionar
                if (!listSymbols.Any(s => s.Contains("-BRL")))
                {
                    listSymbols = listSymbols.Select(s => $"{s}-BRL").ToList();
                }
                
                var symbols = string.Join(",", listSymbols);
                var response = await _httpClient.GetAsync($"tickers?symbols={symbols}");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var tickersRequest = JsonSerializer.Deserialize<List<TickerRequest>>(content);

                return tickersRequest ?? null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to fetch tickers. {ex.Message}");
            }
        }
    }
}
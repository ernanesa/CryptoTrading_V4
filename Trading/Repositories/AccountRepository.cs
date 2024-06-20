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
    }
}
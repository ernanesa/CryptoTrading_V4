using Trading.Entities;
using Trading.Repositories;

namespace Trading.Services
{
    public class AccountService
    {
        private readonly AccountRepository _accountRepository;

        public AccountService(AccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<List<Balance>> GetAccountBalancesAsync(User user)
        {
            return await _accountRepository.GetAccountBalancesAsync(user);
        }

        public async Task<decimal> GetTotalBalanceAsync(User user)
        {
            return await _accountRepository.GetTotalBalanceAsync(user);
        }

        public async Task<Balance> GetAccountBalanceAvailableInRealAsync(User user)
        {
            return await _accountRepository.GetAccountBalanceAvailableInRealAsync(user);
        }
    }
}
using BankingSolution.Models;

namespace BankingSolution.Services
{
    public interface IAccountService
    {
        Task<Account> CreateAccountAsync(Account account);
        Task<Account?> GetAccountByNumberAsync(string accountNumber);
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task UpdateAccountAsync(Account account);
    }
}
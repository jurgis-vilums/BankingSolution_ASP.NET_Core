using BankingSolution.Models;


namespace BankingSolution.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> CreateAsync(Account account);
        Task<Account?> GetByAccountNumberAsync(string accountNumber);
        Task<IEnumerable<Account>> GetAllAsync();
        Task UpdateAsync(Account account);
    }
}
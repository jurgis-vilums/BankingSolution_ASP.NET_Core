using BankingSolution.Models;
using BankingSolution.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingSolution.Services
{
    public class AccountService(IAccountRepository accountRepository) : IAccountService
    {
        public async Task<Account> CreateAccountAsync(Account account)
        {
            return await accountRepository.CreateAsync(account);
        }

        public async Task<Account?> GetAccountByNumberAsync(string accountNumber)
        {
            return await accountRepository.GetByAccountNumberAsync(accountNumber);
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await accountRepository.GetAllAsync();
        }

        public async Task UpdateAccountAsync(Account account)
        {
            await accountRepository.UpdateAsync(account);
        }
    }
}
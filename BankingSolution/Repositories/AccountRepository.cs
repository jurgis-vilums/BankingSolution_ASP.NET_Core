using BankingSolution.Data;
using BankingSolution.Models;
using MongoDB.Driver;

namespace BankingSolution.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MongoDbContext _context;

        public AccountRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<Account> CreateAsync(Account account)
        {
            try
            {
                await _context.Accounts.InsertOneAsync(account);
                return account;
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                throw new Exception("An account with this account number already exists.");
            }
        }

        public async Task<Account?> GetByAccountNumberAsync(string accountNumber)
        {
            return await _context.Accounts.Find(a => a.AccountNumber == accountNumber).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _context.Accounts.Find(_ => true).ToListAsync();
        }

        public async Task UpdateAsync(Account account)
        {
            await _context.Accounts.ReplaceOneAsync(a => a.Id == account.Id, account);
        }
    }
}

using BankingSolution.Models;
using MongoDB.Driver;

namespace BankingSolution.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);

            var accountsCollection = _database.GetCollection<Account>("Accounts");
            var indexKeysDefinition = Builders<Account>.IndexKeys.Ascending(account => account.AccountNumber);
            var indexOptions = new CreateIndexOptions { Unique = true };
            var indexModel = new CreateIndexModel<Account>(indexKeysDefinition, indexOptions);
            accountsCollection.Indexes.CreateOne(indexModel);
        }

        public virtual IMongoCollection<Account> Accounts => _database.GetCollection<Account>("Accounts");
        public virtual IMongoCollection<Transaction> Transactions => _database.GetCollection<Transaction>("Transactions");
    }
}

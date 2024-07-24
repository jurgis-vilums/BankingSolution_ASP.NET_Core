using MongoDB.Bson.Serialization.Attributes;


namespace BankingSolution.Models
{
    public class Transaction
    {
        [BsonElement("Account")] 
        public string Account { get; init; } = string.Empty;

        [BsonElement("Amount")]
        public decimal Amount { get; init; } = 0.0m;

        [BsonElement("TransactionDate")]
        public DateTime TransactionDate { get; init; } = DateTime.UtcNow;
    }
}
using MongoDB.Bson.Serialization.Attributes;


namespace BankingSolution.Models
{
    public class Transfer
    {
        public string FromAccount { get; init; } = string.Empty;
        public string ToAccount { get; init; } = string.Empty;
        public decimal Amount { get; init; }  = 0.0m;
        
        [BsonElement("TransactionDate")]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    }
}
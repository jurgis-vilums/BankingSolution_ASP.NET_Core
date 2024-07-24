using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BankingSolution.Models
{
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("AccountNumber")]
        [Required]
        public required string AccountNumber { get; set; }

        [BsonElement("Balance")]
        [Required]
        public decimal Balance { get; set; }

        [BsonElement("Name")]
        public string? Name { get; set; }
    }
}

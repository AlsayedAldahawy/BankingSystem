using BankingSystem.Utilities;

namespace BankingSystem.Models
{
    public class Transaction
    {
        public string? Id { get; set; }
        
        // ID of the account sending the transaction
        public int AccountId { get; set; }
        
        // Transaction type (e.g., withdrawal, deposit, transfer)
        public required string TransactionType { get; set; }
        
        // Amount involved in the transaction
        public decimal Amount { get; set; }
        
        // Timestamp of the transaction
        public DateTime Timestamp { get; set; }

        // Parameterless constructor for EF Core 
        public Transaction() { } 
        
        // Constructor with parameters
        public Transaction(string trsType="")
        {
            TransactionType = trsType;
            Id = TransactionIdGenerator.GenerateTransactionId(TransactionType);
            
        }
    }
}


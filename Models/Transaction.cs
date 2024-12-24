using BankingSystem.Utilities;

namespace BankingSystem.Models
{
    public class Transaction
    {
        public string Id { get; set; }
        
        // ID of the account sending the transaction
        public int AccountId { get; set; }
        
        // Transaction type (e.g., withdrawal, deposit, transfer)
        public required string TransactionType { get; set; }
        
        // Amount involved in the transaction
        public decimal Amount { get; set; }
        
        // Timestamp of the transaction
        public DateTime Timestamp { get; set; }

        public Transaction()
        {
            Id = TransactionIdGenerator.GenerateTransactionId(TransactionType);
            
        }
    }
}


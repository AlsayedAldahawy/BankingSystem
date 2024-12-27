using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BankingSystem.Utilities;

namespace BankingSystem.Models
{
    public abstract class Transaction
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public required string TransCode { get; set; }
        public required int AccountId { get; set; }
        // Transaction type (e.g., withdrawal, deposit, transfer)
        [MaxLength(250)]
        public required string TransactionType { get; set; }
        
        // Amount involved in the transaction
        public decimal Amount { get; set; }
        
        // Timestamp of the transaction
        public DateTime Timestamp { get; set; }

    }

    public class Transfer : Transaction
    {

        // ID of the account recieving the transaction
        public int RecieverAccountId { get; set; }

    }

    public class Deposit : Transaction { }

    public class Withdraw : Transaction { }

}


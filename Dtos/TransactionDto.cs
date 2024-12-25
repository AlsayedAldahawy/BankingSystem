using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Dtos
{
    public class TransactionDto
    {
        public int SenderAccountId { get; set; }

        // ID of the account recieving the transaction
        public int RecieverAccountId { get; set; }

        // Transaction type (e.g., withdrawal, deposit, transfer)
        [MaxLength(250)]
        public required string TransactionType { get; set; }

        // Amount involved in the transaction
        public decimal Amount { get; set; }
    }
}

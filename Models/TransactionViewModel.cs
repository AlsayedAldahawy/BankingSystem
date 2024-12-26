using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Models
{
    public class TransactionViewModel
    {
        [Key] public int Id { get; set; }
        [MaxLength(100)] public required string TransCode { get; set; }
        public required int AccountId { get; set; }
        [MaxLength(250)] public required string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public int RecieverAccountId { get; set; }
    }

}

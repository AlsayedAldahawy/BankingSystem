using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Dtos
{
    public abstract class TransactionDto
    {
        public int Id { get; set; }
        public string? TransCode { get; set; }
        public int AccountId { get; set; }
        public string? TransactionType { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class TransferDto : TransactionDto
    {
        public int RecieverAccountId { get; set; }

    }

    public class DepositDto : TransactionDto { }
    public class WithdrawDto : TransactionDto { }


}

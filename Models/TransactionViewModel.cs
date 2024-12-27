namespace BankingSystem.Models
{
    public class TransactionViewModel
    {
        public int Id { get; set; }
        public string TransCode { get; set; }
        public int AccountId { get; set; }
        public required string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public int? RecieverAccountId { get; set; }
    }

}

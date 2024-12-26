using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Dtos
{
    public class TransferDto
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public int RecieverAccountId { get; set; }
    }
}

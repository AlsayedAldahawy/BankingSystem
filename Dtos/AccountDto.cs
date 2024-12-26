using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Dtos
{
    public class AccountDto
    {
        public required string AccountHolderName { get; set; }
        public required decimal Balance { get; set; }
        public required string AccountType { get; set; }
        public decimal? InterestRate { get; set; }
        public decimal? OverdraftLimit { get; set; }

    }
}

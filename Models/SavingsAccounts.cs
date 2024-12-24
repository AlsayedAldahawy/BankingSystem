using BankingSystem.Utilities;

namespace BankingSystem.Models
{
    public class SavingsAccount : Account
    {
        public SavingsAccount()
        {
            AccountType = "Savings Account";
            AccountNumber = AccountNumberGenerator.GenerateAccountNumber(AccountType);
        }
        public decimal InterestRate { get; set; }
    }
}

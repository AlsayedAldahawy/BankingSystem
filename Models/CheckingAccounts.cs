using BankingSystem.Utilities;

namespace BankingSystem.Models
{
    public class CheckingAccount : Account
    {

        public CheckingAccount()
        {
            AccountType = "Checking Account";
            AccountNumber = AccountNumberGenerator.GenerateAccountNumber(AccountType);
        }
        public decimal OverdraftLimit { get; set; }
    }
}

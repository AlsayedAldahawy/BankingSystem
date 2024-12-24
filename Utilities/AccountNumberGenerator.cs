namespace BankingSystem.Utilities
{
    public class AccountNumberGenerator
    {
        private static int counter = 1000; // A counter to ensure uniqueness
        
        public static string GenerateAccountNumber(string accountType) { 
            string prefix = accountType == "CheckingAccount" ? "CHK" : "SAV"; 
            return $"{prefix}-{counter++}"; 
        }
    }
}

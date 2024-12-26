namespace BankingSystem.Utilities
{
    public class AccountNumberGenerator
    {
        
        public static string GenerateAccountNumber(string accountType) {

            string prefix = accountType == "CheckingAccount" ? "CHK" : "SAV";

            int counter = IdStorage.ReadJsonValue(prefix);

            IdStorage.WriteJsonValue(prefix, counter + 1);

            return $"{prefix}-{counter}"; 
        }
    }
}

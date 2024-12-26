namespace BankingSystem.Utilities
{
    public class TransactionIdGenerator
    {        
        public static string GenerateTransactionId(string TransactionType) { 
            string prefix="";
            
            switch (TransactionType)
            {
                case "Withdrawal": 
                    prefix = "WTH";
                    break;
                case "Deposit":
                    prefix = "DEP";
                    break;

                case "Transfer":
                    prefix = "TRN";
                    break;
            };

            int counter = IdStorage.ReadJsonValue(prefix);

            IdStorage.WriteJsonValue(prefix, counter);

            return $"{prefix}{counter}"; 
        }
    }
}

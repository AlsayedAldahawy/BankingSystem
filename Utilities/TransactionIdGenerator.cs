namespace BankingSystem.Utilities
{
    public class TransactionIdGenerator
    {        
        public static string GenerateTransactionId(string TransactionType) { 
            string prefix="";
            
            switch (TransactionType)
            {
                case "Withdraw": 
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

            IdStorage.WriteJsonValue(prefix, counter + 1);

            return $"{prefix}{counter}"; 
        }
    }
}

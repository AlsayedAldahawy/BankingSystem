namespace BankingSystem.Utilities
{
    public class TransactionIdGenerator
    {
        private static int counter = 1000; // A counter to ensure uniqueness
        
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

                case "Transfer Out":
                    prefix = "OUT";
                    break;

                case "Transfer In":
                    prefix = "IN";
                    break;
            };

            return $"{prefix}-{counter++}"; 
        }
    }
}

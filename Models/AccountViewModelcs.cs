namespace BankingSystem.Models
{
    public class AccountViewModel { 
        public int Id { get; set; } 
        public string AccountNumber { get; set; } 
        public string AccountHolderName { get; set; } 
        public decimal Balance { get; set; } 
        public string AccountType { get; set; } 
        public decimal? OverdraftLimit { get; set; } 
        public decimal? InterestRate { get; set; } }
}

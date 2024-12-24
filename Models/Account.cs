using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Models
{
    
    public abstract class Account
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string AccountNumber { get; set; }

        public string AccountHolderName { get; set; }
        public decimal Balance { get; set; }
        public string AccountType { get; set; }

    }
}

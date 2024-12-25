using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Models
{
    
    public abstract class Account
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string AccountNumber { get; set; }
        public required string AccountHolderName { get; set; }
        public decimal Balance { get; set; }
        public required string AccountType { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using BankingSystem.Utilities;

namespace BankingSystem.Models
{
    public class SavingsAccount : Account
    {
        [MaxLength(100)]

        public decimal InterestRate { get; set; }
    }
}

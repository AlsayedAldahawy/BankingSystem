using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using BankingSystem.Utilities;

namespace BankingSystem.Models
{
    public class CheckingAccount : Account
    {
        [MaxLength(100)]
        public decimal OverdraftLimit { get; set; }
    }
}

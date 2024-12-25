﻿using System.ComponentModel.DataAnnotations;
using BankingSystem.Utilities;

namespace BankingSystem.Models
{
    public class Transaction
    {
        public string? Id { get; set; }
        
        // ID of the account sending the transaction
        public int SenderAccountId { get; set; }

        // ID of the account recieving the transaction
        public int RecieverAccountId { get; set; }

        // Transaction type (e.g., withdrawal, deposit, transfer)
        [MaxLength(250)]
        public required string TransactionType { get; set; }
        
        // Amount involved in the transaction
        public decimal Amount { get; set; }
        
        // Timestamp of the transaction
        public DateTime Timestamp { get; set; }

        // Parameterless constructor for EF Core 
        
        // Constructor with parameters
    }
}


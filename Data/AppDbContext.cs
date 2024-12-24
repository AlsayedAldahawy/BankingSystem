using BankingSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BankingSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }


        // Specify precision and scale
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasDiscriminator<string>("AccountType")
                .HasValue<CheckingAccount>("Checking Account")
                .HasValue<SavingsAccount>("Savings Account");

            modelBuilder.Entity<Account>()
                .Property(a => a.Balance)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<CheckingAccount>()
                .Property(ca => ca.OverdraftLimit)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<SavingsAccount>()
                .Property(sa => sa.InterestRate)
                .HasColumnType("decimal(5,2)");

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");

            // Seed data for CheckingAccount
            modelBuilder.Entity<CheckingAccount>().HasData(
                new CheckingAccount
                {
                    Id = 1,
                    AccountNumber = "CHK123456",
                    Balance = 1500.00m,
                    AccountType = "Checking Account",
                    AccountHolderName = "John Doe",
                    OverdraftLimit = 500.00m
                },
                new CheckingAccount
                {
                    Id = 2,
                    AccountNumber = "CHK654321",
                    Balance = 1200.00m,
                    AccountType = "Checking Account",
                    AccountHolderName = "Jane Doe",
                    OverdraftLimit = 400.00m
                }
            );

            // Seed data for SavingsAccount
            modelBuilder.Entity<SavingsAccount>().HasData(
                new SavingsAccount
                {
                    Id = 3,
                    AccountNumber = "SAV123456",
                    Balance = 5000.00m,
                    AccountType = "Savings Account",
                    AccountHolderName = "Alice Smith",
                    InterestRate = 2.5m
                },
                new SavingsAccount
                {
                    Id = 4,
                    AccountNumber = "SAV654321",
                    Balance = 8000.00m,
                    AccountType = "Savings Account",
                    AccountHolderName = "Bob Johnson",
                    InterestRate = 2.0m
                }
            );

            // Seed data for Transaction
            modelBuilder.Entity<Transaction>().HasData(
                new Transaction
                {
                    Id = "DEP999",
                    AccountId = 1,
                    TransactionType = "Deposit",
                    Amount = 500.00m,
                    Timestamp = DateTime.Now
                },
                new Transaction
                {
                    Id = "WTH548",
                    AccountId = 3,
                    TransactionType = "Withdrawal",
                    Amount = 200.00m,
                    Timestamp = DateTime.Now
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}



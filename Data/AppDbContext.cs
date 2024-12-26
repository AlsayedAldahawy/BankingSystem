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

            modelBuilder.Entity<Transaction>()
                .HasDiscriminator<string>("TransactionType")
                .HasValue<Deposit>("Deposit")
                .HasValue<Withdraw>("Withdraw")
                .HasValue<Transfer>("Transfer");

            modelBuilder.Entity<Account>()
                .Property(a => a.Balance)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<CheckingAccount>()
                .Property(ca => ca.OverdraftLimit)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<SavingsAccount>()
                .Property(sa => sa.InterestRate)
                .HasColumnType("decimal(5,2)");

            modelBuilder.Entity<Transfer>()
                .Property(sa => sa.RecieverAccountId)
                .HasColumnType("int");

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

            // Seed data for Transfer
            modelBuilder.Entity<Transfer>().HasData(
                new Transfer
                {
                    Id= 1,
                    AccountId = 1,
                    TransCode = "TRN1550",
                    RecieverAccountId = 2,
                    TransactionType = "Deposit",
                    Amount = 500.00m,
                    Timestamp = DateTime.Now
                },
                new Transfer
                {
                    Id= 2,
                    AccountId= 3,
                    TransCode = "TRN1555",
                    RecieverAccountId = 2,
                    TransactionType = "Withdraw",
                    Amount = 200.00m,
                    Timestamp = DateTime.Now
                }
                );

            // Seed data for Deposit
            modelBuilder.Entity<Deposit>().HasData(
                new Deposit
                {
                    Id = 3,
                    TransCode = "DPO1550",
                    AccountId = 1,
                    TransactionType = "Deposit",
                    Amount = 500.00m,
                    Timestamp = DateTime.Now
                },
                new Deposit
                {
                    Id = 4,
                    TransCode = "DPO1555",
                    AccountId= 2,
                    TransactionType = "Deposit",
                    Amount = 200.00m,
                    Timestamp = DateTime.Now
                }
                );

            // Seed data for Deposit
            modelBuilder.Entity<Withdraw>().HasData(
                new Withdraw
                {
                    Id= 5,
                    TransCode = "WTH1550",
                    AccountId = 1,
                    TransactionType = "Withdraw",
                    Amount = 500.00m,
                    Timestamp = DateTime.Now
                },
                new Withdraw
                {
                    Id = 6,
                    TransCode = "WTH1555",
                    AccountId = 2,
                    TransactionType = "Withdraw",
                    Amount = 200.00m,
                    Timestamp = DateTime.Now
                }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}



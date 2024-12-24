using Microsoft.AspNetCore.Mvc;
using BankingSystem.Data;
using BankingSystem.Models;
using Microsoft.EntityFrameworkCore;


namespace BankingSystem.Controllers
{
    public class AccountsController : Controller
    {
        private readonly AppDbContext _context;

        public AccountsController(AppDbContext context)
        {
            _context = context;
        }

        // List all accounts
        public async Task<IActionResult> Index()
        {
            var accounts = await _context.Accounts.ToListAsync();
            return View(accounts);
        }

        // Deposit money into an account
        [HttpPost]
        public async Task<IActionResult> Deposit(int accountId, decimal amount)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null)
                return NotFound();

            account.Balance += amount;
            _context.Transactions.Add(new Transaction("Deposit")
            {
                AccountId = account.Id,
                Amount = amount,
                TransactionType = "Deposit",
                Timestamp = DateTime.Now
            });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Withdraw money from an account
        [HttpPost]
        public async Task<IActionResult> Withdraw(int accountId, decimal amount)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null)
                return NotFound();

            if (account is CheckingAccount checkingAccount)
            {
                var maxWithdrawAmount = checkingAccount.Balance + checkingAccount.OverdraftLimit;
                if (amount > maxWithdrawAmount)
                    return BadRequest("Insufficient funds.");

                checkingAccount.Balance -= amount;
            }
            else if (account is SavingsAccount savingsAccount)
            {
                if (amount > savingsAccount.Balance)
                    return BadRequest("Insufficient funds.");

                savingsAccount.Balance -= amount;
            }

            _context.Transactions.Add(new Transaction("Withdrawal")
            {
                AccountId = account.Id,
                Amount = amount,
                TransactionType = "Withdrawal",
                Timestamp = DateTime.Now
            });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Transfer money between accounts
        [HttpPost]
        public async Task<IActionResult> Transfer(int sourceAccountId, int targetAccountId, decimal amount)
        {
            var sourceAccount = await _context.Accounts.FindAsync(sourceAccountId);
            var targetAccount = await _context.Accounts.FindAsync(targetAccountId);

            if (sourceAccount == null || targetAccount == null)
                return NotFound();

            if (sourceAccount.Balance < amount)
                return BadRequest("Insufficient funds in the source account.");

            sourceAccount.Balance -= amount;
            targetAccount.Balance += amount;

            _context.Transactions.Add(new Transaction("Transfer Out")
            {
                AccountId = sourceAccount.Id,
                Amount = amount,
                TransactionType = "Transfer Out",
                Timestamp = DateTime.Now
            });

            _context.Transactions.Add(new Transaction("Transfer In")
            {
                AccountId = targetAccount.Id,
                Amount = amount,
                TransactionType = "Transfer In",
                Timestamp = DateTime.Now
            });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Retrieve account balance
        [HttpGet]
        public async Task<IActionResult> GetBalance(int accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null)
                return NotFound();

            return Ok(account.Balance);
        }
    }
}

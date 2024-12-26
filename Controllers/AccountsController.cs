using BankingSystem.Data;
using BankingSystem.Dtos;
using BankingSystem.Models;
using BankingSystem.Utilities;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {


        private readonly AppDbContext _context;


        public AccountsController(AppDbContext context)
        {
            _context = context;
        }

        // GET allAccounts
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var accounts = await _context.Accounts.OrderBy(a => a.Id).ToListAsync();
            var detailedAccounts = accounts.Select(a => new
            {
                a.Id,
                a.AccountNumber,
                a.Balance,
                a.AccountType,
                AccountHolderName = (a is CheckingAccount ca) ? ca.AccountHolderName : ((SavingsAccount)a).AccountHolderName,
                OverdraftLimit = a is CheckingAccount ? ((CheckingAccount)a).OverdraftLimit : (decimal?)null,
                InterestRate = a is SavingsAccount ? ((SavingsAccount)a).InterestRate : (decimal?)null
            })
                .ToList(); return Ok(detailedAccounts);
        }

        //* GET GetByIdAsync
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(a => a.Id == id);
            if (account == null)
            {
                return NotFound($"No Account with ID: {id}");
            }
            var detailedAccount = new
            {
                account.Id,
                account.AccountNumber,
                account.Balance,
                account.AccountType,
                AccountHolderName = (account is CheckingAccount ca) ? ca.AccountHolderName : ((SavingsAccount)account).AccountHolderName,
                OverdraftLimit = account is CheckingAccount ? ((CheckingAccount)account).OverdraftLimit : (decimal?)null,
                InterestRate = account is SavingsAccount ? ((SavingsAccount)account).InterestRate : (decimal?)null
            }; 
            
            return Ok(detailedAccount);
        }

        // * GET api/accounts?getByType=AccountType -- GetByTypeasync
        [HttpGet("GetByType")]
        public async Task<IActionResult> GetByTypeAsync([FromQuery] string type)
        {
            if (type.ToLower() != "checking" && type.ToLower() != "savings")
                return BadRequest($"Invalid Account Type: {type}");

            var accounts = await _context.Accounts
                .Where(a => a.AccountType == $"{type} Account")
                .OrderByDescending(a => a.Id).ToListAsync();

            return Ok(accounts);
        }

        // Get Account Balance
        [HttpGet("{id}/balance")]
        public async Task<IActionResult> GetBalanceAsync(int id)
        {
            var account = await _context.Accounts
                .SingleOrDefaultAsync(a => a.Id == id);

            if (account == null)
                return NotFound($"No Account with Number: {id}");

            return Ok(account.Balance);
        }


        // *POST AddAsync
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromForm] AccountDto dto)
        {
            // Data Verification
            if (dto.Balance < 0)
                return BadRequest("Balance Can not be negative number");

            if (dto.AccountType.ToLower() == "checking account")
            {
                if (dto.OverdraftLimit == null || dto.OverdraftLimit.Value < 0)
                    return BadRequest("Invalid OverdraftLimit value");

                var account = new CheckingAccount
                {
                    AccountType = "Checking Account",
                    AccountNumber = AccountNumberGenerator.GenerateAccountNumber("CheckingAccount"),
                    AccountHolderName = dto.AccountHolderName,
                    Balance = dto.Balance,
                    OverdraftLimit = dto.OverdraftLimit.HasValue ? dto.OverdraftLimit.Value : dto.Balance * 0.20m,
                };

                await _context.Accounts.AddAsync(account);
                _context.SaveChanges();
                return Ok(account);
            }
            else if (dto.AccountType.ToLower() == "savings account")
            {
                if (dto.InterestRate == null || dto.InterestRate.Value < 0)
                    return BadRequest("Invalid InterestRate value");

                var account = new SavingsAccount
                {
                    AccountType = "Savings Account",
                    AccountNumber = AccountNumberGenerator.GenerateAccountNumber("SavingsAccount"),
                    AccountHolderName = dto.AccountHolderName,
                    Balance = dto.Balance,
                    InterestRate = dto.InterestRate.Value,
                };
                await _context.Accounts.AddAsync(account);
                _context.SaveChanges();
                return Ok(account);
            }
            else
                return BadRequest("Invalid Account Type");
        }

        //Deposit money into an account
        [HttpPut("deposit")]
        public async Task<IActionResult> Deposit([FromBody] DepositDTO dto)
        {
            if (dto.Amount < 0)
                return BadRequest("Ammount of Money can't be negative");

            var account = await _context.Accounts.FindAsync(dto.AccountId);

            if (account == null)
                return NotFound($"No acount with id: {dto.AccountId}");

            account.Balance += dto.Amount;

            _context.Transactions.Add(new Deposit
            {
                TransCode = TransactionIdGenerator.GenerateTransactionId("Deposit"),
                AccountId = dto.AccountId,
                Amount = dto.Amount,
                TransactionType = "Deposit",
                Timestamp = DateTime.Now,
            }
            );

            await _context.SaveChangesAsync();
            return Ok(account);
        }

        //Withdraw money from an account
        [HttpPut("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] DepositDTO dto)
        {
            if (dto.Amount < 0)
                return BadRequest("Ammount of Money can't be negative");
            var account = await _context.Accounts.FindAsync(dto.AccountId);

            if (account == null)
                return NotFound($"No acount with id: {dto.AccountId}");

            if (dto.Amount > account.Balance)
            {
                if (account.AccountType == "Savings Account")
                    return BadRequest("No Enough Balance");

                if (account.AccountType == "Checking Account")
                {
                    var checkingAccount = account as CheckingAccount;
                    if (checkingAccount == null)
                        return BadRequest("Failed Process: Account type mismatch.");
                    if (dto.Amount - account.Balance > checkingAccount.OverdraftLimit)
                        return BadRequest("Failed Process: Withdraw amount exceeds Overdraft limit.");
                }
            }

            account.Balance -= dto.Amount;

            _context.Transactions.Add(new Withdraw
            {
                TransCode = TransactionIdGenerator.GenerateTransactionId("Withdraw"),
                AccountId = dto.AccountId,
                Amount = dto.Amount,
                TransactionType = "Withdraw",
                Timestamp = DateTime.Now,
            }
            );

            await _context.SaveChangesAsync();
            return Ok(account);
        }


        //Transfer money between two accounts
        [HttpPut("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferDto dto)
        {
            if (dto.Amount < 0)
                return BadRequest("Ammount of Money can't be negative");

            var sender = await _context.Accounts.FindAsync(dto.AccountId);
            var reciever = await _context.Accounts.FindAsync(dto.RecieverAccountId);

            if (sender == null)
                return NotFound($"No acount with id: {dto.AccountId}");

            if (reciever == null)
                return NotFound($"No acount with id: {dto.RecieverAccountId}");

            if (dto.Amount > sender.Balance)
            {
                if (sender.AccountType == "Savings Account")
                    return BadRequest("No Enough Balance");

                if (sender.AccountType == "Checking Account")
                {
                    var checkingAccount = sender as CheckingAccount;
                    if (checkingAccount == null)
                        return BadRequest("Failed Process: Account type mismatch.");
                    if (dto.Amount - sender.Balance > checkingAccount.OverdraftLimit)
                        return BadRequest("Failed Process: Withdraw amount exceeds Overdraft limit.");
                }
            }

            sender.Balance -= dto.Amount;
            reciever.Balance += dto.Amount;

            _context.Transactions.Add(new Transfer
            {
                TransCode = TransactionIdGenerator.GenerateTransactionId("Transfer"),
                AccountId = dto.AccountId,
                RecieverAccountId = dto.RecieverAccountId,
                Amount = dto.Amount,
                TransactionType = "Transfer",
                Timestamp = DateTime.Now,
            }
            );

            await _context.SaveChangesAsync();
            return Ok(sender);
        }


        // Delete Account

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
                return NotFound($"No acount with id: {id}");

            _context.Accounts.Remove(account);
            _context.SaveChanges();
            return Ok(account);
        }
    }
}

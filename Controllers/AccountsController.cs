using BankingSystem.Data;
using BankingSystem.Dtos;
using BankingSystem.Models;
using BankingSystem.Utilities;
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

            return Ok(accounts);
        }

        //* GET GetByIdAsync
        [HttpGet("{number}")]
        public async Task<IActionResult> GetByIdAsync(string number)
        {
            var transaction = await _context.Accounts
                .SingleOrDefaultAsync(a => a.AccountNumber == number);

            if (transaction == null)
                return NotFound($"No Account with Number: {number}");

            return Ok(transaction);
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
    }
}

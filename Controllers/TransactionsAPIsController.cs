using System.Security.Cryptography;
using BankingSystem.Data;
using BankingSystem.Dtos;
using BankingSystem.Models;
using BankingSystem.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


/**
 * 
 * 
 * 
 * 
 **/

namespace BankingSystem.Controllers
{
    [Route("api/Transactions")]
    [ApiController]
    public class TransactionsAPIsController : ControllerBase
    {
        private readonly AppDbContext _context;

        
        public TransactionsAPIsController (AppDbContext context)
        {
            _context = context;
        }

        //* GET api/transactions    -- GetAllAsync
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var transactions = await _context.Transactions.OrderBy(g => g.Timestamp).ToListAsync();
            return Ok(transactions);
        }


         //* GET api/transaction/{id}  -- GetByIdAsync
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id) 
        {
            var transaction = await _context.Transactions
                .SingleOrDefaultAsync(t => t.Id == id); 

            if (transaction == null) 
                return NotFound($"There's no Transaction with id: {id}");

            if (transaction.TransactionType != "Transfer")
            {
                return Ok(new DepositDto
                {
                    Id = transaction.Id,
                    TransactionType = transaction.TransactionType,
                    Amount = transaction.Amount,
                    AccountId = transaction.AccountId,
                    Timestamp = transaction.Timestamp,
                    TransCode = transaction.TransCode,
                });
            }
            return Ok(transaction);
        }

        //* GET api/transactions/GetByAccountId? accountId = { id }  -- GetByAccountAsync
        [HttpGet("GetByAccountId")]
        public async Task<IActionResult> GetByAccountAsync([FromQuery] int accountId)
        {
            var transactions = await _context.Transactions
                .Where(t => t.AccountId == accountId).OrderByDescending(t => t.Timestamp).ToListAsync();

            if (transactions == null || transactions.Count == 0)
                return NotFound($"No Transactions with this Account id: {accountId}");

            return Ok(transactions);
        }


        // * GET api/transactions?getByType=transactionType -- GetTransactionsByTypeasync
        [HttpGet("GetByType")]
        public async Task<IActionResult> GetByTypeAsync([FromQuery] string Ttype)
        {
            var transactions = await _context.Transactions
                .Where(t => t.TransactionType == Ttype)
                .OrderByDescending(t => t.Timestamp).ToListAsync();

            if (transactions == null || transactions.Count == 0)
                return NotFound($"No Transactions with this Type: {Ttype}");

            return Ok(transactions);
        }

        ////* POST api/transactions[FromBody] -- AddAsync
        ///delayed untill I implement deposit, withdraw, transfer apis of the accounts
        //[HttpPost]
        //public async Task<IActionResult> AddTransactionAsync(TransactionDto dto)
        //{
        //    if (dto.Amount < 0)
        //        return BadRequest("Amount of Transfer can not be negative number");

        //    if (dto.SenderAccountId == dto.RecieverAccountId)
        //        return BadRequest("Sender ID and Reciever Id can't be the same");



        //    var sender = await _context.Accounts.FindAsync(dto.SenderAccountId);
        //    var reciever = await _context.Accounts.FindAsync(dto.RecieverAccountId);

        //    if (sender == null)
        //        return BadRequest("Invalid Sender ID");
        //    if (reciever == null)
        //        return BadRequest("Invalid Reciever ID");

        //    var Trasaction = new Transaction
        //    {
        //        Id = TransactionIdGenerator.GenerateTransactionId(dto.TransactionType),
        //        SenderAccountId = dto.SenderAccountId,
        //        TransactionType = dto.TransactionType,
        //        RecieverAccountId = dto.RecieverAccountId,
        //        Amount = dto.Amount,
        //        Timestamp = DateTime.Now

        //    };

        //    sender.Balance -= dto.Amount;
        //    reciever.Balance += dto.Amount;

        //    await _context.Transactions.AddAsync(Trasaction);
        //    _context.SaveChanges();

        //    return Ok(Trasaction);

        //}

        //  * PUT api/transactions/{id}         -- UpdateAsync

        //[HttpPut("{tid}")]
        //public async Task<IActionResult> UpdateAsync(string tid, [FromForm] TransactionDto dto)
        //{
        //    var transaction = await _context.Transactions
        //        .SingleOrDefaultAsync(t => t.Id == tid);

        //    if (transaction == null)
        //        return NotFound($"No Available Transaction with id: {tid}");




        //}

        //  * DELETE api/transactions/{id}      -- DeleteAsync
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionAsync(int id)
        {
            var transaction = await _context.Transactions
                .SingleOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
                return NotFound($"There's no Transaction with id: {id}");

            _context.Transactions.Remove(transaction);
            _context.SaveChanges();
            return Ok(transaction);
        }
    }
}

using BankingSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly AppDbContext _context;

        public TransactionsController(AppDbContext context)
        {
            _context = context;
        }

        // List all Transactions
        public async Task<IActionResult> Index()
        {
            var transactions = await _context.Transactions.ToListAsync();
            return View(transactions);
        }
    }
}

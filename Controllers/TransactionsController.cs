using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BankingSystem.Models;

namespace BankingSystem.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly HttpClient _httpClient;

        public TransactionsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetStringAsync("http://localhost:5195/api/transactions");
            var transactions = JsonConvert.DeserializeObject<List<TransactionViewModel>>(response);
            return View(transactions);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5195/api/transactions/{id}");
            return RedirectToAction("Index");
        }
    }

}

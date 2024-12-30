using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BankingSystem.Models;
using System.Text;

namespace BankingSystem.Controllers
{
    public class AccountsController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: /Accounts
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetStringAsync("http://localhost:5195/api/accounts");
            var accounts = JsonConvert.DeserializeObject<List<AccountViewModel>>(response);
            return View(accounts);
        }

        [HttpPost]
        [Route("Accounts/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5195/api/accounts/delete/{id}");
            return RedirectToAction("Index");
        }

        // GET: /Accounts/Details/5
        [HttpGet]
        [Route("Accounts/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetStringAsync($"http://localhost:5195/api/accounts/{id}");
            var account = JsonConvert.DeserializeObject<AccountViewModel>(response);
            return View(account);
        }
    }
}

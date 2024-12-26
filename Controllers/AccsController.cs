using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BankingSystem.Dtos;
using BankingSystem.Models;

namespace BankingSystem.Controllers
{
    public class AccsController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetStringAsync("https://localhost:7095/api/accounts");
            var accounts = JsonConvert.DeserializeObject<List<AccountViewModel>>(response);
            return View(accounts);
        }

        [HttpPost]
        public async Task<IActionResult> AddAccount([FromForm] AccountViewModel account)
        {
            var formContent = new FormUrlEncodedContent(new[] {
        new KeyValuePair<string, string>("AccountNumber", account.AccountNumber),
        new KeyValuePair<string, string>("AccountHolderName", account.AccountHolderName),
        new KeyValuePair<string, string>("Balance", account.Balance.ToString()),
        new KeyValuePair<string, string>("AccountType", account.AccountType),
        new KeyValuePair<string, string>("OverdraftLimit", account.OverdraftLimit.ToString()),
        new KeyValuePair<string, string>("InterestRate", account.InterestRate.ToString())
    });
            var response = await _httpClient.PostAsync("https://localhost:7095/api/accounts", formContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return BadRequest();
        }

        //[HttpDelete]
        //public async Task<IActionResult> DeleteAccount(int id)
        //{
        //    var response = await _httpClient.DeleteAsync($"https://localhost:7095/api/delete/{id}");
        //    if (response.IsSuccessStatusCode) { return RedirectToAction("Index"); }
        //    return BadRequest();
        //}
    }



}

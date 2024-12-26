﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
            var response = await _httpClient.GetStringAsync("http://localhost:5195/api/accounts");
            var transactions = JsonConvert.DeserializeObject<List<AccountViewModel>>(response);
            return View(transactions);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5195/api/accounts/{id}");
            return RedirectToAction("Index");
        }
    }

}

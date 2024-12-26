using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers
{
    public class Accounts : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

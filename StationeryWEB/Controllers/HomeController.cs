using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging; // Ensure this using directive is included for ILogger

namespace StationeryWEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;

        // Single constructor that takes both ILogger and IHttpClientFactory
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ToAdmin()
        {
            return View("Admin");
        }
        public IActionResult ToDealer()
        {
            return View("Dealer");
        }
        public IActionResult ToUser()
        {
            return View("User");
        }
        public IActionResult ToDetailPage()
        {
            return View("ItemDetails");
        }
        // LOGIN FORMS
        public IActionResult ToAdminLogin()
        {
            return View("LoginAdmin");
        }
        public IActionResult ToUserLogin()
        {
            return View("LoginUser");
        }
        public IActionResult ToDealerLogin()
        {
            return View("LoginDealer");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}

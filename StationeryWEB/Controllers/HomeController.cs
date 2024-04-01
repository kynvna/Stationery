using Microsoft.AspNetCore.Mvc;
using StationeryWEB.Models;
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

        public async Task<IActionResult> Index()
        {
            //var client = _clientFactory.CreateClient();
            //var response = await client.GetAsync("https://localhost:7106/weatherforecast");
            //if (response.IsSuccessStatusCode)
            //{
            //    var jsonString = await response.Content.ReadAsStringAsync();
            //    var weatherData = JsonConvert.DeserializeObject<IEnumerable<WeatherForecast>>(jsonString);
            //    // Now you have your weather data and you can pass it to the view
            //    return View(weatherData);
            //}
           
                var client = _clientFactory.CreateClient();
                var response = await client.GetAsync("https://localhost:7106/api/DemoCore");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
               

                var demoCoreData = JsonConvert.DeserializeObject<DemoCoreViewModel>(jsonString);
                    return View(demoCoreData);
                    
                }
                

            return View("Error");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

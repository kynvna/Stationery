using Microsoft.AspNetCore.Mvc;
using StationeryWEB.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using StationeryAPI.ShoppingModels;
using System.Text.Json;
using System.Text; // For Encoding.UTF8


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

        //public async Task<IActionResult> Index()
        //{
        //    //var client = _clientFactory.CreateClient();
        //    //var response = await client.GetAsync("https://localhost:7106/weatherforecast");
        //    //if (response.IsSuccessStatusCode)
        //    //{
        //    //    var jsonString = await response.Content.ReadAsStringAsync();
        //    //    var weatherData = JsonConvert.DeserializeObject<IEnumerable<WeatherForecast>>(jsonString);
        //    //    // Now you have your weather data and you can pass it to the view
        //    //    return View(weatherData);
        //    //}

        //        var client = _clientFactory.CreateClient();
        //        var response = await client.GetAsync("https://localhost:7106/api/Customer/GetProducts");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var jsonString = await response.Content.ReadAsStringAsync();


        //        var products = JsonConvert.DeserializeObject<ProductPageViewModel>(jsonString);

        //        return View(products);

        //        }


        //    return View("Error");
        //}

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7106/api/Customer/GetProducts?page={page}&pageSize={pageSize}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<ProductPageViewModel>(jsonString);
                return View(products);
            }

            return View("Error");
        }
        //-------------------create new order----------------------//
        [HttpGet]
        public IActionResult CreateOrder()
        {
            return View(new NewOrder()); // Pass a new instance to the view for the form
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrder(NewOrder order)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                string url = "https://localhost:7106/api/Customer/CreateOrder";

                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    // Assuming you want to redirect the user to the Index action after successful order creation
                    return RedirectToAction("Index");
                }
                else
                {
                    // Log error response and possibly display an error message to the user
                    _logger.LogError("Failed to create order. Status code: {StatusCode}", response.StatusCode);
                    return View("Error"); // Make sure you have an Error view or handle this appropriately
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while creating the order.");
                return View("Error"); // Handle exceptions
            }
        }

        //--------------------------------------------------------//
        [HttpGet]
        public IActionResult CreateOrderDetail()
        {
            return View(new StationeryWEB.Models.OrderDetail()); // Pass a new instance to the view for the form
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrderDetail(StationeryWEB.Models.OrderDetail orderDetail)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(orderDetail);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                string url = "https://localhost:7106/api/Customer/CreateOrderDetail";

                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    // Assuming you want to redirect the user to the Index action after successful order creation
                    return RedirectToAction("Index");
                }
                else
                {
                    // Log error response and possibly display an error message to the user
                    _logger.LogError("Failed to create order. Status code: {StatusCode}", response.StatusCode);
                    return View("Error"); // Make sure you have an Error view or handle this appropriately
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while creating the order.");
                return View("Error"); // Handle exceptions
            }
        }

        //-------------------------------------------------------//
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

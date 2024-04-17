using Microsoft.AspNetCore.Mvc;
using StationeryWEB.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using StationeryAPI.ShoppingModels;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Net.Http.Headers;
using System.Globalization;


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
        //Product - Admin
        public async Task<IActionResult> AdminIndex(int page = 1, int pageSize = 10)
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

        //----------------Update order by id -----------------------//
        [HttpGet]
        public async Task<IActionResult> UpdateOrder()
        {
            
            var client = _clientFactory.CreateClient();
            string id = Request.Query["id"];
            ViewBag.id = id;

            var response = await client.GetAsync($"https://localhost:7106/api/Customer/GetOrder/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<UpdateOrder>(responseData);
                // Now you can return the order object to your view or handle it as needed
                return View(res);
            }
            else
            {
                // Handle null (deserialization failed or JSON is empty)
                return View();
            }
            return View(new UpdateOrder()); // Pass a new instance to the view for the form
        }
        public IActionResult AdminProfile()
        {
            return View();
        }
        public IActionResult DealerProfile()
        {
            return View(); 
        }
        public IActionResult Login()
        {
            return View(); 
        }
        public IActionResult Register()
        {
            return View(); 
        }
        public async Task<IActionResult> loginUser(LoginModel model)
        {
            string webApiUrl = "https://localhost:7106/api/Dealer/login";
            string json = JsonConvert.SerializeObject(model);
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(webApiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                LoginSuccess login = JsonConvert.DeserializeObject<LoginSuccess>(responseData);
                if (login.role == 0)
                {
                    ViewBag.id = login.id;
                    return Redirect("/Home/AdminProfile");
                }
                else
                {
                    return Redirect("/Home/DealerProfile");
                }

            }
            return Redirect("/Home/Index");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrder(UpdateOrder order)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                string url = "https://localhost:7106/api/Customer/UpdateOrder";

                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    // Assuming you want to redirect the user to the Index action after successful order creation
                    return RedirectToAction("UpdateOrder");
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

        //--------------------------Delete an order----------------------//

        public async Task<IActionResult> GetOrder(string orderId)
        {
            var client = _clientFactory.CreateClient();

            //string json = Newtonsoft.Json.JsonConvert.SerializeObject(orderId);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");
            //string url = "https://localhost:7106/api/Customer/GetOrder/{orderId}";

            //var response = await client.PostAsync(url, content);


            var response = await client.GetAsync($"https://localhost:7106/api/Customer/GetOrder/{orderId}");

            if (response.IsSuccessStatusCode)
                {
               
                    // Now you can return the order object to your view or handle it as needed
                    return View(response);
                }
                else
                {
                    // Handle null (deserialization failed or JSON is empty)
                    return View();
                }
           
        }


        [HttpPost]
        public async Task<IActionResult> DeleteOrder(string orderId)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7106/api/Customer/DeleteOrder/{orderId}");

            if (response.IsSuccessStatusCode)
            {
                // Redirect to a confirmation page or back to the order list
                return RedirectToAction("DeleteOrder");
            }
            else
            {
                // Handle the error or return to the order with an error message
                return View("ErrorDeletingOrder");
            }
        }

        //----------get order by dealer--------------------------//
        [HttpGet]
        public IActionResult GetOrderByDealer()
        {
            // Initialize your model with an empty list of TblOrder within OrderValues
            var model = new OrderViewModel
            {

                Orders = new List<StationeryAPI.ShoppingModels.TblOrder>(), // Initialize the nested list
                
                TotalOrders = 0, // Default initialization values
                TotalPages = 0,
                CurrentPage = 1,
                PageSize = 10
            };

            return View(model);
        }




        [HttpPost]
        public async Task<IActionResult> GetOrderByDealer(string? dealerId, int page = 1, int pageSize = 10)
        {
            var client = _clientFactory.CreateClient();
            var url = $"https://localhost:7106/api/Customer/GetOrderByDealer?page={page}&pageSize={pageSize}";

            // Append deaId to the URL if it's not null, using HttpUtility.UrlEncode for URL encoding
            if (dealerId != null)
            {
                url += $"&dealerId={System.Net.WebUtility.UrlEncode(dealerId)}";
            }

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                try
                {
                    var orders = JsonConvert.DeserializeObject<OrderViewModel>(jsonString);
                    return View(orders);
                }
                catch (JsonSerializationException ex)
                {   
                    Console.WriteLine(ex.Message);
                    // Handle the exception, possibly log it, and return an error view
                    return View("Error");
                }
            }

            return View("Error");
        }
        //--------------------------------------------------------//
        [HttpGet]
        public async Task<IActionResult> CreateOrderDetail()
        {
            var client = _clientFactory.CreateClient();
            string id = Request.Query["id"];
            var response = await client.GetAsync($"https://localhost:7106/api/Customer/CreateOrderDetail");
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<UpdateDelivery>(responseData);
                ViewBag.id = id;
                return View(res);
            }
            else
            {
                // Handle null (deserialization failed or JSON is empty)
                return View();
            }
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

        //------------------Delivery----------------------------//
      
       
        public async Task<IActionResult> Deliveries(int page = 1, int pageSize = 10)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7106/api/Customer/Deliveries?page={page}&pageSize={pageSize}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var deliveries = JsonConvert.DeserializeObject<DeliveryViewModel>(jsonString);
                return View(deliveries);
                

            }

            return View("Error");
        }
        //---------------------Delivery by dealderID-----------------//
        [HttpGet]
        public IActionResult DeliveriesByDealer()
        {

            var model = new DeliveryViewModel
            {
                Deliveries = new List<TblDelivery>() // Assuming Delivery is the correct type; adjust as necessary
            };
            return View(model); // Pass the initialized model to the view
        }

        [HttpPost]
        public async Task<IActionResult> DeliveriesByDealer(string? dealerId, int page = 1, int pageSize = 10)
        {
            var client = _clientFactory.CreateClient();
            var url = $"https://localhost:7106/api/Customer/DeliveriesByDealer?page={page}&pageSize={pageSize}";

            // Append deaId to the URL if it's not null, using HttpUtility.UrlEncode for URL encoding
            if (dealerId != null)
            {
                url += $"&dealerId={System.Net.WebUtility.UrlEncode(dealerId)}";
            }

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var deliveries = JsonConvert.DeserializeObject<DeliveryViewModel>(jsonString);
                return View(deliveries);
            }

            return View("Error");
        }
      
        //--------------------------Update delivery          -----------------------------//
        [HttpGet]
        public async Task<IActionResult> UpdateDeliveryAsync()
        {
            var client = _clientFactory.CreateClient();
            string id = Request.Query["id"];
            ViewBag.id = id;

            var response = await client.GetAsync($"https://localhost:7106/api/Customer/GetDelivery/{id}");

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<UpdateDelivery>(responseData);
                return View(res);
            }
            else
            {
                // Handle null (deserialization failed or JSON is empty)
                return View();
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateDelivery(UpdateDelivery updatedelivery)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(updatedelivery);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                string url = "https://localhost:7106/api/Customer/UpdateDelivery";

                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    // Assuming you want to redirect the user to the Index action after successful order creation
                    return RedirectToAction("UpdateDelivery");
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

        //------------------------------delete a delivery------------------------------//

        public async Task<IActionResult> GetDelivery(string deliveryId)
        {
            var client = _clientFactory.CreateClient();

            var response = await client.GetAsync($"https://localhost:7106/api/Customer/GetOrder/{deliveryId}");

            if (response.IsSuccessStatusCode)
            {

                // Now you can return the order object to your view or handle it as needed
                return View(response);
            }
            else
            {
                // Handle null (deserialization failed or JSON is empty)
                return View();
            }

        }


        [HttpPost]
        public async Task<IActionResult> DeleteDelivery(string deliveryId)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7106/api/Customer/DeleteDelivery/{deliveryId}");

            if (response.IsSuccessStatusCode)
            {
                // Redirect to a confirmation page or back to the order list
                return RedirectToAction("DeleteDelivery");
            }
            else
            {
                // Handle the error or return to the order with an error message
                return View("ErrorDeletingOrder");
            }
        }

        //-----------------------------------------------------------------------------//
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

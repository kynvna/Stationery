using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
<<<<<<< HEAD
using Microsoft.Extensions.Logging;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq; // Ensure this using directive is included for ILogger
=======
using System.Text;
using System.Text.Json.Serialization;
using System.Net.Http.Headers;
using System.Globalization;

>>>>>>> origin/dangvnh1504

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

<<<<<<< HEAD
        public IActionResult Index()
=======
        

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
            var response = await client.GetAsync($"https://localhost:7106/api/Customer/GetOrderDetail/{id}");
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
      
        //--------------------------Update delivery  -----------------------------//
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




        //-----------------------------Get all customerproduct--------------------------------//

        public async Task<IActionResult> GetCustomerProduct(int page = 1, int pageSize = 10)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7106/api/Customer/GetCustomerProduct?page={page}&pageSize={pageSize}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var customerproducts = JsonConvert.DeserializeObject<CustomerProductViewModel>(jsonString);
                return View(customerproducts);


            }
            return View("Error");
        }

        //-------------------------------Creating a customerproduct-----------------------//
        [HttpGet]
        public IActionResult CreateCustomerProduct()
        {
            return View(new NewCustomerProduct()); // Pass a new instance to the view for the form
        }


        [HttpPost]
        public async Task<IActionResult> CreateCustomerProduct(NewCustomerProduct customerProduct)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(customerProduct);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                string url = "https://localhost:7106/api/Customer/CreateCustomerProduct";

                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    // Assuming you want to redirect the user to the Index action after successful order creation
                    return RedirectToAction("CreateCustomerProduct");
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
                _logger.LogError(ex, "An error occurred while creating the customerproduct.");
                return View("Error"); // Handle exceptions
            }
        }
        //---------------------------------Get & Delete a CustomerProductByID-----------------------//

        public async Task<IActionResult> GetCustomerProductById(string custId,string ProId)
        {
            var client = _clientFactory.CreateClient();

            

            var response = await client.GetAsync($"https://localhost:7106/api/Customer/GetOrder/{custId}&{ProId}");

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
        public async Task<IActionResult> DeleteCustomerProduct(string custId,string ProId)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7106/api/Customer/DeleteCustomerProduct/{custId}&{ProId}");

            if (response.IsSuccessStatusCode)
            {
                // Redirect to a confirmation page or back to the order list
                return RedirectToAction("DeleteCustomerOrder");
            }
            else
            {
                // Handle the error or return to the order with an error message
                return View("ErrorDeletingOrder");
            }
        }

        //-------------------------------- Get CustomerProductByDealer-----------------------------------------------//

        [HttpGet]
        public IActionResult GetCustomerProductByDealer()
        {
            
            var model = new CustomerProductViewModel
            {

                CustomerProducts = new List<StationeryAPI.ShoppingModels.TblCustomerProduct>(), // Initialize the nested list

                TotalCustomerProducts = 0, // Default initialization values
                TotalPages = 0,
                CurrentPage = 1,
                PageSize = 10
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> GetCustomerProductByDealer(string? dealerId, int page = 1, int pageSize = 10)
        {
            var client = _clientFactory.CreateClient();
            var url = $"https://localhost:7106/api/Customer/GetCustomerProductByDealer?page={page}&pageSize={pageSize}";

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
                    var customerproducts = JsonConvert.DeserializeObject<CustomerProductViewModel>(jsonString);
                    return View(customerproducts);
                }
                catch (JsonSerializationException ex)
                {
                    // Handle the exception, possibly log it, and return an error view
                    return View("Error");
                }
            }

            return View("Error");
        }

        //--------------------------------------Update customerproduct------------------------------//
        [HttpGet]
        public IActionResult UpdateCustomerProduct()
        {
            return View(new UpdateCustomerProduct()); // Pass a new instance to the view for the form
        }


        [HttpPost]
        public async Task<IActionResult> UpdateCustomerProduct(UpdateCustomerProduct updatecustomerproduct)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(updatecustomerproduct);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                string url = "https://localhost:7106/api/Customer/Updateupdatecustomerproduct";

                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    // Assuming you want to redirect the user to the Index action after successful order creation
                    return RedirectToAction("UpdateCustomerProduc");
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

        //---------------------------------customer order form---------------------------------------//
        [HttpGet]
        public async Task<IActionResult> RegisterProductById(string id)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7106/api/Customer/RegisterProductById/{id}");

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<NewCustomerProduct>(responseData); // Assuming the use of Newtonsoft.Json and a 'Product' model

                TempData["NewCustomerProduct"] = product;
                return View("CreateCustomerProduct"); // Corrected to return a view with the data
            }
            else
            {
                // It's a good practice to provide error details or logging here
                ViewBag.ErrorMessage = "Failed to retrieve product details. Please try again.";
                return View("Error"); // Redirect to an error view or similar approach
            }
        }



        //[HttpPost]
        //public async Task<IActionResult> RegisterProductById(NewCustomerProduct newcustomerproduct)
        //{
        //    try
        //    {
        //        var client = _clientFactory.CreateClient();
        //        string json = Newtonsoft.Json.JsonConvert.SerializeObject(newcustomerproduct);
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");
        //        string url = "https://localhost:7106/api/Customer/CreateCustomerProduct";

        //        var response = await client.PostAsync(url, content);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            // Assuming you want to redirect the user to the Index action after successful order creation
        //            return RedirectToAction("RegisterProductById");
        //        }
        //        else
        //        {
        //            // Log error response and possibly display an error message to the user
        //            _logger.LogError("Failed to create order. Status code: {StatusCode}", response.StatusCode);
        //            return View("Error"); // Make sure you have an Error view or handle this appropriately
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception
        //        _logger.LogError(ex, "An error occurred while creating the order.");
        //        return View("Error"); // Handle exceptions
        //    }
        //}

        //------------------------------------------------------------------------------------------//
        public IActionResult Privacy()
>>>>>>> origin/dangvnh1504
        {
            string path = "https://dummyjson.com/products";
            object sir = GetItem(path);
            JObject jObject = JObject.Parse(sir.ToString());
            ViewBag.data = jObject["products"];
            return View();
        }
        public object GetItem(string path)
        {
            using (WebClient webClient = new WebClient())
            {
                return JsonConvert.DeserializeObject<object>(
                    webClient.DownloadString(path)
                );
            }
        }
        //--- ADMIN --------------------------------
        public IActionResult ToAdmin()
        {
            return View("Admin");
        }
        public IActionResult ToAdminDealer()
        {
            return View("AdminDealerView");
        }
        public IActionResult ToAdminUser()
        {
            return View("AdminUserView");
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

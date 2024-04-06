using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StationeryAPI.ShoppingModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StationeryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ShoppingWebContext _context;

        public CustomerController(ShoppingWebContext context)
        {
            _context = context;
        }

        public static string GenerateOrderNumberUsingTicks()
        {
            // Get current ticks
            long ticks = DateTime.UtcNow.Ticks;

            // Convert to string and take the last 8 digits
            string orderNumberStr = ticks.ToString().Substring(Math.Max(0, ticks.ToString().Length - 8));

            return orderNumberStr;
        }


        [HttpGet("GetProducts")]
        public ActionResult Get([FromQuery] int page=1 , [FromQuery] int pageSize=10 )
        {
            // Calculate the total number of products
            int totalProducts = _context.TblProducts.Count();

            // Calculate the total number of pages
            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            // Fetch the products for the requested page
            var products = _context.TblProducts
                                   .Skip((page - 1) * pageSize) // Skip the products from the previous pages
                                   .Take(pageSize)              // Take the next 'pageSize' number of products
                                   .ToList();                   // Execute the query and get the list of products

            // Prepare the response
            var response = new
            {
                TotalProducts = totalProducts,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                Products = products
            };

            return Ok(response);
        }

        //----------------------- Post an new order----------------------//
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> Post([FromBody] NewOrder newOrder)
        {
            if (newOrder == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
               var newTblOrder= new TblOrder();
                // You might need to set some properties like OrderId if they are not auto-generated
                //newTblOrder.OrderId = Guid.NewGuid().ToString("N"); // Produces a 32-character hexadecimal string.
                newTblOrder.OrderId = GenerateOrderNumberUsingTicks();                                                 // Or your logic to generate OrderId
                newTblOrder.OrderDate = DateTime.UtcNow; // Or use the date passed in if it should be set client-side
                newTblOrder.OrderStatus=newOrder.OrderStatus;
                newTblOrder.CustomerId=newOrder.CustomerId;
                newTblOrder.TotalPrice= newOrder.TotalPrice;
                // Add the new order to the database context
                _context.TblOrders.Add(newTblOrder);

                // Save the changes to the database
                await _context.SaveChangesAsync();

                // Return a success response, perhaps with the created order object
                // and the URL where the order can be accessed (e.g., Get Order by ID)
                return CreatedAtAction(nameof(GetOrder), new { id = newTblOrder.OrderId }, newTblOrder);
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, "An error occurred while creating the order.");
            }
        }

        // Make sure you have a corresponding Get action that matches the CreatedAtAction
        // for example:
        [HttpGet("GetOrder{id}")]
        public async Task<IActionResult> GetOrder(string id)
        {
            var order = await _context.TblOrders
                                      .Include(o => o.TblDeliveries)
                                      .Include(o => o.TblOrderDetails)
                                      .Include(o => o.TblReviews)
                                      .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        //----------------------- Post an new orderdetail----------------------//
        [HttpPost("CreateOrderDetail")]
        public async Task<IActionResult> Post([FromBody] OderDetail orderDetail)
        {
            if (orderDetail == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newTblOrderDetail = new TblOrderDetail();
                // You might need to set some properties like OrderId if they are not auto-generated
                //newTblOrder.OrderId = Guid.NewGuid().ToString("N"); // Produces a 32-character hexadecimal string.
                                                                   // Or your logic to generate OrderId
                newTblOrderDetail.OrderId = orderDetail.OrderId; // Or use the date passed in if it should be set client-side
                newTblOrderDetail.ProductId = orderDetail.ProductId;
                newTblOrderDetail.Quantity = orderDetail.Quantity;
                newTblOrderDetail.Discount = orderDetail.Discount; 
                newTblOrderDetail.TotalAmount = orderDetail.TotalAmount;

                // Add the new order to the database context
                _context.TblOrderDetails.Add(newTblOrderDetail);

                // Save the changes to the database
                await _context.SaveChangesAsync();

                // Return a success response, perhaps with the created order object
                // and the URL where the order can be accessed (e.g., Get Order by ID)
                return CreatedAtAction(nameof(GetOrderDetail), new { id = newTblOrderDetail.DetailId }, newTblOrderDetail);
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, "An error occurred while creating the order.");
            }
        }

        // Make sure you have a corresponding Get action that matches the CreatedAtAction
        // for example:
        [HttpGet("GetOrderDetail{id}")]
        public async Task<IActionResult> GetOrderDetail(string id)
        {
            var order = await _context.TblOrders
                                      .Include(o => o.TblDeliveries)
                                      .Include(o => o.TblOrderDetails)
                                      .Include(o => o.TblReviews)
                                      .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
        //---------------------

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CustomerController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CustomerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

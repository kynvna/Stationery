using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StationeryAPI.ShoppingModels;
using StationeryWEB.Models;

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

        // --------------------Get all product API---------------------//
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

        //----------------------- Post a new order <add in availability check>----------------------//
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
                
                //newTblOrder.OrderId = Guid.NewGuid().ToString("N"); // Produces a 32-character hexadecimal string.
                newTblOrder.OrderId = GenerateOrderNumberUsingTicks();
                newTblOrder.OrderDate = DateTime.UtcNow.AddHours(7);
                // Or use the date passed in if it should be set client-side
                newTblOrder.productId = newOrder.productId;
                newTblOrder.OrderStatus=newOrder.OrderStatus;
                newTblOrder.CustomerId=newOrder.CustomerId;
                newTblOrder.TotalPrice= newOrder.TotalPrice;
                newTblOrder.DeliveryType = newOrder.DeliveryType;
                newTblOrder.DeliveryFee = newOrder.DeliveryFee;
                newTblOrder.PaymentMethod = newOrder.PaymentMethod;
                newTblOrder.PaymentStatus = newOrder.PaymentStatus;
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


        // --------------------Get an Order by id-------------------------------//
        [HttpGet("GetOrder/{id}")]
        public async Task<IActionResult> GetOrder(string id)
        {
            var order = await _context.TblOrders
                                      .Include(o => o.TblDeliveries)
                                      .Include(o => o.TblOrderDetails)
                                       .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
            }
            //---------------------Update Order by id--------------------------------//


        [HttpPost("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder([FromBody] StationeryWEB.Models.UpdateOrder updateOrder)
        {
            if (updateOrder == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var order = _context.TblOrders.FirstOrDefault(o => o.OrderId == updateOrder.OrderId);

                if (order != null)
                {

                    order.OrderDate = updateOrder.OrderDate; // Or use the date passed in if it should be set client-side
                    order.productId = updateOrder.productId;
                    order.OrderStatus = updateOrder.OrderStatus;
                    order.CustomerId = updateOrder.CustomerId;
                    order.TotalPrice = updateOrder.TotalPrice;
                    order.DeliveryType = updateOrder.DeliveryType;
                    order.DeliveryFee = updateOrder.DeliveryFee;
                    order.PaymentMethod = updateOrder.PaymentMethod;
                    order.PaymentStatus = updateOrder.PaymentStatus;

                    // Save the changes to the database
                    await _context.SaveChangesAsync();
                    UpdateOrder returnorder = OrderConverter.ConvertToUpdateOrder(order);
                    // Return a success response, perhaps with the created order object
                    // and the URL where the order can be accessed (e.g., Get Order by ID)
                    return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, returnorder );
                }
                else return (null);
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, "An error occurred while creating the order.");
            }
        }


        //----------------Get Order by dealerID-------------------------------//
        [HttpGet("GetOrderByDealer")]
        public ActionResult GetOrderByDealer([FromQuery] string dealerId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
           
            
            var orders = _context.TblOrders
                     .Include(order => order.Product)
                     .Where(order => order.Product.dealerId == dealerId)
                     .ToList();

            int totalOrders = orders.Count();

            // Calculate the total number of pages
            var totalPages = (int)Math.Ceiling(totalOrders / (double)pageSize);

            // Fetch the products for the requested page
            var orderbydealer = orders
                                   .Skip((page - 1) * pageSize) // Skip the products from the previous pages
                                   .Take(pageSize)              // Take the next 'pageSize' number of products
                                   .ToList();                   // Execute the query and get the list of products

            // Prepare the response
            var response = new
            {
                TotalOrders = totalOrders,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                Orders = orderbydealer
            };

            return Ok(response);
        }

        //------------------------------Delete an order--------------------------//
        [HttpDelete("DeleteOrder/{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            var order = await _context.TblOrders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.TblOrders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent(); // Returns a 204 No Content response
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
               
                //newTblOrder.OrderId = Guid.NewGuid().ToString("N"); // Produces a 32-character hexadecimal string.
                                                                   
                newTblOrderDetail.OrderId = orderDetail.OrderId; 
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

        
        //---------------- Get an orderdetail by id --------------------//
        [HttpGet("GetOrderDetail{id}")]
        public async Task<IActionResult> GetOrderDetail(string id)
        {
            var order = await _context.TblOrders
                                      .Include(o => o.TblDeliveries)
                                      .Include(o => o.TblOrderDetails)
                                      
                                      .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
        //--------------------------Get all deliveries-----------------------------------//

        [HttpGet("Deliveries")]
        public ActionResult Deliveries([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            // Calculate the total number of products
            
            int totalDeliveries = _context.TblDeliveries.Count();

            // Calculate the total number of pages
            var totalPages = (int)Math.Ceiling(totalDeliveries / (double)pageSize);

            // Fetch the products for the requested page
            var deliveries = _context.TblDeliveries
                                   .Skip((page - 1) * pageSize) // Skip the products from the previous pages
                                   .Take(pageSize)              // Take the next 'pageSize' number of products
                                   .ToList();                   // Execute the query and get the list of products

            // Prepare the response
            var response = new
            {
                TotalDeliveries = totalDeliveries,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                Deliveries = deliveries
            };

            return Ok(response);
        }
        //----------------------get delivery by dealerId----------------------------//
        [HttpGet("DeliveriesByDealer")]
        public ActionResult DeliveriesByDealer([FromQuery] string dealerId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            // Calculate the total number of products
            var orderIds = _context.TblOrders
                            .Join(_context.TblProducts, // Join Orders with Products
                                  order => order.productId,
                                  product => product.ProId,
                                  (order, product) => new { order, product })
                            .Where(op => op.product.dealerId == dealerId)
                            .Select(op => op.order.OrderId)
                            .Distinct()
                            .ToList();

            // Step 2: Find all Deliveries associated with the filtered Orders
            var filteredDeliveries = _context.TblDeliveries
                                              .Where(delivery => orderIds.Contains(delivery.OrderId))
                                              .OrderBy(delivery => delivery.DeliveryId); // Adjust according to your schema
                                              //.Skip((page - 1) * pageSize)
                                              //.Take(pageSize)
                                              //.ToList();
            int totalDeliveries = filteredDeliveries.Count();

            // Calculate the total number of pages
            var totalPages = (int)Math.Ceiling(totalDeliveries / (double)pageSize);

            // Fetch the products for the requested page
            var deliveries = filteredDeliveries
                                   .Skip((page - 1) * pageSize) // Skip the products from the previous pages
                                   .Take(pageSize)              // Take the next 'pageSize' number of products
                                   .ToList();                   // Execute the query and get the list of products

            // Prepare the response
            var response = new
            {
                TotalDeliveries = totalDeliveries,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                Deliveries = deliveries
            };

            return Ok(response);
        }

        // ------------------update delivery by id-----------------------------//
        // GET: api/Delivery/{id}
        [HttpGet("GetDelivery/{id}")]
        public async Task<ActionResult<TblDelivery>> GetDelivery(string id)
        {
            // Assuming 'DeliveryId' is the name of your primary key property in the TblDelivery entity
            var delivery = await _context.TblDeliveries.FindAsync(id);

            if (delivery == null)
            {
                return NotFound();
            }

            return delivery;
        }

        [HttpPost("UpdateDelivery")]
        public async Task<IActionResult> UpdateDelivery([FromBody] UpdateDelivery updateDelivery)
        {
            if (updateDelivery == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var delivery = _context.TblDeliveries.FirstOrDefault(o => o.DeliveryId == updateDelivery.DeliveryId);

                if (delivery != null)
                {

                     // Or use the date passed in if it should be set client-side
                    delivery.DeliveryDate  = updateDelivery.DeliveryDate;
                    delivery.DeliveryFee   = updateDelivery.DeliveryFee;
                    delivery.DeliveryStatus = updateDelivery.DeliveryStatus;
                    delivery.CarrierName= updateDelivery.CarrierName;
                    delivery.OrderId= updateDelivery.OrderId;

                    // Save the changes to the database
                    await _context.SaveChangesAsync();
                   
                    return CreatedAtAction(nameof(GetDelivery), new { id = delivery.DeliveryId }, delivery);
                }
                else return (null);
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, "An error occurred while creating the order.");
            }
        }


        //----------------------------Delete delivery by id------------------------------------------//
        [HttpDelete("DeleteDelivery/{id}")]
        public async Task<IActionResult> DeleteDelivery(string id)
        {
            var delivery = await _context.TblDeliveries.FindAsync(id);
            if (delivery == null)
            {
                return NotFound();
            }

            _context.TblDeliveries.Remove(delivery);
            await _context.SaveChangesAsync();

            return NoContent(); // Returns a 204 No Content response
        }

        //-----------------------------------------------------------------------//

    }
    public static class OrderConverter
    {
        public static UpdateOrder ConvertToUpdateOrder(TblOrder tblOrder)
        {
            if (tblOrder == null)
            {
                throw new ArgumentNullException(nameof(tblOrder), "TblOrder cannot be null");
            }

            var updateOrder = new UpdateOrder
            {
                OrderId = tblOrder.OrderId,
                productId = tblOrder.productId,
                CustomerId = tblOrder.CustomerId,
                OrderDate = tblOrder.OrderDate,
                TotalPrice = tblOrder.TotalPrice,
                OrderStatus = tblOrder.OrderStatus,
                DeliveryType = tblOrder.DeliveryType,
                DeliveryFee = tblOrder.DeliveryFee,
                PaymentMethod = tblOrder.PaymentMethod,
                PaymentStatus = tblOrder.PaymentStatus,
            };

            return updateOrder;
        }
    }
}

using demo_jwt_netcore.Models;
using demo_jwt_netcore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StationeryAPI.ShoppingModels;
using StationeryAPI.Models;
using StationeryAPI.Utils;

namespace StationeryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealerController : ControllerBase
    {
        ShoppingWebContext db = new ShoppingWebContext();
        private readonly IConfiguration _config;
        private readonly IUserService _userService;

        //constructor
        public DealerController(IConfiguration config)
        {
            _config = config;
            _userService = new UserService();
        }

        [HttpPost("login")]
        public IActionResult Login(LoginModel loginModel)
        {
            //Authenticate user
            var user = _userService.Authenticate(loginModel.Username, loginModel.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            //Generate tokens
            var accessToken = TokenUtils.GenerateAccessToken(user, "LopHocThayKhiem_demo_jwt_Aa@!@#$%^&*()");
            var refreshToken = TokenUtils.GenerateRefreshToken();

            var response = new TokenResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Role = user.Role,
                id = user.UserId
            };
            return Ok(response);
        }

        [HttpGet("getproductbydealer")]
        public async Task<IActionResult> GetProductByDealer(string id ,[FromQuery]  int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                
                int totalProducts = db.TblProducts.Count();

                
                var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

                
                var products = db.TblProducts.Where(d => d.DealerId == id)
                                       .Skip((page - 1) * pageSize) // Skip the products from the previous pages
                                       .Take(pageSize)              // Take the next 'pageSize' number of products
                                       .ToList();

                if (products.Count == 0)
                {
                    return NotFound("No active categories found");
                }

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
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Or log the error for debugging
            }
        }
        public static string GenerateOrderNumberUsingTicks()
        {
            // Get current ticks
            long ticks = DateTime.UtcNow.Ticks;

            // Convert to string and take the last 8 digits
            string orderNumberStr = ticks.ToString().Substring(Math.Max(0, ticks.ToString().Length - 7));

            return orderNumberStr;
        }

        [HttpPost("createProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreate danhMuc)
        {
            
            var productcode = db.TblCategories.ToList();
            TblProduct tblProduct = new TblProduct();
            try
            {
                // Xác thực mô hình danh mục (giả sử có chú thích dữ liệu hoặc trình xác thực)
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Trả về lỗi xác thực
                }
                foreach (var category in productcode)
                {
                    string product = category.CatId;
                    long ticks = DateTime.UtcNow.Ticks;

                    // Convert to string and take the last 8 digits
                    string orderNumberStr = ticks.ToString().Substring(Math.Max(0, ticks.ToString().Length - 5));
                    string productid = product + orderNumberStr;
                    DateTime Date = DateTime.Now;
                    tblProduct.ProId = productid;
                    tblProduct.ProName = danhMuc.ProName;
                    tblProduct.ProStatus = danhMuc.ProStatus;
                    tblProduct.ProQuantity = danhMuc.ProQuantity;
                    tblProduct.Price = danhMuc.Price;
                    tblProduct.ImageLink = danhMuc.ImageLink;
                    tblProduct.Description = danhMuc.Description;
                    tblProduct.TimeCreated = Date;
                    tblProduct.CatId = category.CatId;
                    tblProduct.DealerId = danhMuc.DealerId;
                    await db.TblProducts.AddAsync(tblProduct);
                    await db.SaveChangesAsync();
                }


                // Thêm danh mục mới vào ngữ cảnh cơ sở dữ liệu


                // Lưu thay đổi vào cơ sở dữ liệu


                // Trả về danh mục vừa tạo (tùy chọn)
                /* return CreatedAtRoute("GetCategoryById", new { id = danhMuc.Id }, danhMuc);*/

                // Hoặc trả về một thông báo thành công đơn giản
                return Ok("Danh mục đã được tạo thành công");
            }
            catch (Exception ex)
            {
                // Ghi nhật ký lỗi để gỡ lỗi

                return StatusCode(500, "Lỗi máy chủ nội bộ"); // Trả về thông báo lỗi chung
            }
        }
        [HttpGet("getProductid")]
        public async Task<IActionResult> GetProductId(string id)
        {
            try
            {
                var product = await db.TblProducts.FindAsync(id);
                if (product == null)
                {
                    return NotFound("No product");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Or log the error for debugging
            }
        }
        [HttpPut("updateproduct")]
        public bool UpdateProducty(UpdateProduct model)
        {
            bool status;
            string id = model.ProId;

            try
            {
                // 1. Input Validation (Enhanced)


                // 2. Retrieve Category (Error Handling)
                TblProduct ProductUpdate = new TblProduct();
                ProductUpdate = db.TblProducts.Find(id);
                if (ProductUpdate != null)
                {
                    ProductUpdate.ProName = model.ProName;
                    ProductUpdate.ProStatus = model.ProStatus;
                    ProductUpdate.ProQuantity = model.ProQuantity;
                    ProductUpdate.ImageLink = model.ImageLink;
                    ProductUpdate.Price = model.Price;
                    ProductUpdate.Description = model.Description;
                    ProductUpdate.TimeUpdated = DateTime.Now;
                    db.TblProducts.Update(ProductUpdate);
                    db.SaveChanges();
                }

                status = true;

            }
            catch
            {
                status = false;
            }
            return status;
        }

        [HttpDelete("deleteproduct")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            bool isDeleted = false;
            try
            {
                var ls = await db.TblProducts.FindAsync(id);
                if (ls != null)
                {
                    db.TblProducts.Remove(ls);
                    db.SaveChanges();
                    isDeleted = true;
                }
                return Ok(isDeleted);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("getUser")]
        public async Task<IActionResult> GetAllUser([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                // Filter active categories
                int totalUser = db.TblAdEmpAccounts.Where(u => u.Role == 1).Count();

                // Calculate total pages
                var totalPages = (int)Math.Ceiling(totalUser / (double)pageSize);


                var users = await db.TblAdEmpAccounts
           .Where(u => u.Role == 1 ) 
           .Skip((page - 1) * pageSize)
           .Take(pageSize)
           .ToListAsync();

                if (users.Count == 0)
                {
                    return NotFound("No user");
                }
                var response = new
                {
                    TotalUser = totalUser,
                    TotalPages = totalPages,
                    CurrentPage = page,
                    PageSize = pageSize,
                    AllUser = users
                };

                // Consider including additional information or pagination here

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Or log the error for debugging
            }
        }

        [HttpPost("CreateDealer")]
        public async Task<IActionResult> CreateDealer([FromBody] DealerAcc model )
        {
            int role = 1;
            long ticks = DateTime.UtcNow.Ticks;

            // Convert to string and take the last 8 digits
            string iduser = ticks.ToString().Substring(Math.Max(0, ticks.ToString().Length - 5));
             
            try
            {
                TblAdEmpAccount dealer = new TblAdEmpAccount();
                dealer.Role = role;
                dealer.Fullname = model.Fullname;
                dealer.Username = model.Username;
                dealer.Active = model.Active;
                dealer.Passw = model.Passw;
                dealer.UserId = iduser;

              await  db.TblAdEmpAccounts.AddAsync(dealer);
                db.SaveChangesAsync();
                return Ok("Da tao user thanh cong");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("getDealerid")]
        public async Task<IActionResult> GetDealerid(string id)
        {
            try
            {
                var user = await db.TblAdEmpAccounts.FindAsync(id);
                if (user == null)
                {
                    return NotFound("no user");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Or log the error for debugging
            }
        }

        [HttpPut("updateDealer")]
        public bool updateDealer( UpdateDealer model)
        {
            int role = 1;
            string dealerid = model.UserId;
            bool status;
            try
            {
                TblAdEmpAccount tblAdEmp = new TblAdEmpAccount();
                tblAdEmp =  db.TblAdEmpAccounts.Find(dealerid);
                if (tblAdEmp == null)
                {
                    return false;
                }
                tblAdEmp.Role = role;
                tblAdEmp.Active = model.Active;
                tblAdEmp.Fullname = model.Fullname;
                tblAdEmp.Username = model.Username;
                tblAdEmp.Passw = model.Passw;
                db.TblAdEmpAccounts.Update(tblAdEmp);
                db.SaveChanges();
                status = true;
            }
            catch
            {
                status = false;
            }
            return status;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDealer(string id)
        {
            bool isDeleted = false;
            try
            {
                var del = await db.TblAdEmpAccounts.FindAsync(id);
                if (del != null)
                {
                    db.TblAdEmpAccounts.Remove(del);
                    db.SaveChanges();
                    isDeleted = true;
                }
                return Ok(isDeleted);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }

}

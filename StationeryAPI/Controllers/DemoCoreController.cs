using Microsoft.AspNetCore.Mvc;
using StationeryAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StationeryAPI.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]
    public class DemoCoreController : ControllerBase
    {

        private readonly DemoCoreContext _context;

        public DemoCoreController(DemoCoreContext context)
        {
            _context = context;
        }
        //[Route("api/democore")]
        [HttpGet]
        public ActionResult Get()
        {
            var departments = _context.Departments.ToList();
            var employees = _context.Employees.ToList();

            // You might want to create a ViewModel or a DTO to transfer data
            var result = new
            {
                Departments = departments,
                Employees = employees
            };

            return Ok(result);
        }

        // GET api/<DemoCoreController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<DemoCoreController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<DemoCoreController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DemoCoreController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

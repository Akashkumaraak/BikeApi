using BikeApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BikeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailController : Controller
    {
        private readonly BikeContext _db;

        public DetailController(BikeContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<ActionResult<OrderDetail>> Index()
        {
            List<OrderDetail> detail = new List<OrderDetail>();
             detail = await _db.OrderDetails.Include(c => c.Product).Include(c => c.OrderMaster).ToListAsync();
            return Ok(detail);
        }
    }
}

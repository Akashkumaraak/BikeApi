using BikeApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BikeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly BikeContext _db;

        public MasterController(BikeContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<ActionResult<OrderMaster>> Index()
        {
            List<OrderMaster> masters = new List<OrderMaster>();
            masters = await _db.OrderMasters.ToListAsync();
            return Ok(masters);
        }
    }
}


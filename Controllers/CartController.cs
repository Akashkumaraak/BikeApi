using BikeApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly BikeContext _db;
        public CartController(BikeContext db)
        {
            _db = db;
        }
        [HttpPost]
        public async Task<ActionResult<Cart>> Create(Cart cart)
        {
            _db.Carts.Add(cart);
            await _db.SaveChangesAsync();
            return Ok();
        }
        [HttpGet]
        public async Task<ActionResult<Cart>> Index()
        {
            List<Cart> cart = new List<Cart>();
            cart = _db.Carts.ToList();
            return Ok(cart);
        }
        [HttpGet]
        [Route("Details")]
        public async Task<ActionResult> Details(int id)
        {
            var book = (from i in _db.Carts
                        where i.Cartid == id
                        select i).FirstOrDefault();
            return Ok(book);
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var cart = await _db.Carts.FindAsync(id);
            _db.Carts.Remove(cart);
            _db.SaveChanges();
            return Ok();
        }
        [HttpPost]
        [Route("ProceedToPay")]
        public async Task<ActionResult<OrderMaster>> ProceedToPay(OrderMaster om)
        {
            _db.OrderMasters.Add(om);
            await _db.SaveChangesAsync();
            return Ok();
        }
        [HttpGet]
        [Route("OrderMasterGetById")]
        public async Task<ActionResult<OrderMaster>> OrderMasterGetById(int id)
        {
            var om = _db.OrderMasters.Find(id);
            return Ok(om);
        }
        [HttpPut]
        public async Task<ActionResult<OrderMaster>> OrderMasterPut(OrderMaster om)
        {
            _db.OrderMasters.Update(om);
            await _db.SaveChangesAsync();
            return Ok();
        }
        [HttpPost]
        [Route("AddRangeOrderDetails")]
        public async Task<ActionResult<OrderDetail>> AddRangeOrderDetails(List<OrderDetail> od)
        {
            _db.OrderDetails.AddRange(od);
            await _db.SaveChangesAsync();
            return Ok();
        }
        [HttpPost]
        [Route("RemoveRangeCart")]
        public async Task<ActionResult<Cart>> RemoveRangeCart(List<Cart> carts)
        {
            _db.Carts.RemoveRange(carts);
            await _db.SaveChangesAsync();
            return Ok();
        }
        [HttpPost]
        [Route("ListConvert")]
        public async Task<ActionResult<Cart>> ListConvert(Cart cart)
        {
            List<Cart> carts = new List<Cart>();
            carts = (from i in _db.Carts
                    where i.Userid == cart.Userid
                    select i).ToList();
            return Ok(carts);
        }
        [HttpPost]
        [Route("Cartvalidation")]
        public async Task<ActionResult<Cart>> Cartvalidation(Cart c)
        {
            var cart = (from i in _db.Carts
                      where i.Userid == c.Userid && i.Productid == c.Productid
                      select i).SingleOrDefault();
            return Ok(cart);
        }
        [HttpGet]
        [Route("Price")]
        public async Task<ActionResult> Price(Cart cart)
        {
            var price = (from i in _db.Products
                        where i.ProductId == cart.Productid
                        select i.Price).SingleOrDefault();
            return Ok(price);
        }
        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult<Cart>> Update(Cart cart)
        {
            _db.Carts.Update(cart);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }   
}
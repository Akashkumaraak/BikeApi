using BikeApi.Models;
using BikeApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BikeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly BikeContext _db;

        public CartsController(BikeContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("GetCartById")]
        public async Task<Cart> GetCartById(int id)
        {
            var result = await _db.Carts.Include(x => x.Product).Where(x => x.Cartid == id).Select(x => x).FirstOrDefaultAsync();
            return result;
        }

        //[HttpGet("{id}")]
        //public async Task<List<Cart>> GetCart(int id)
        //{
        //    List<Cart> cart = new List<Cart>();
        //    var Id = await _db.Users.Where(x => x.UserId == id).Select(x => x.Carts).FirstOrDefaultAsync();
        //    cart = await _db.Carts.Include(x => x.Product).Where(x => x.Cartid = Id ).ToListAsync();
        //    return cart;
        //}


        [HttpPut("{id}")]
        public async Task<ActionResult<Cart>> PutCart(int id, Cart cart)
        {
            var c = await _db.Carts.FindAsync(id);
            c.Quantity = cart.Quantity;
            c.TotalAmount = c.Quantity * (from i in _db.Products where i.ProductId == c.Productid select i.Price).SingleOrDefault();
            _db.Carts.Update(c);
            await _db.SaveChangesAsync();
            return c;
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            if (_db.Carts == null)
            {
                return Problem("Entity set 'BikeContext.Carts'  is null.");
            }
            var cart = await _db.Carts.FindAsync(id);
            if (cart != null)
            {
                _db.Carts.Remove(cart);
            }

            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("AddToCart")]
        public async Task<ActionResult<Cart>> AddToCart(Cart cart)
        {

            var id = (from i in _db.Carts
                      where i.Userid == cart.Userid && i.Productid == cart.Productid
                      select i).SingleOrDefault();
            if (id == null)
            {
                cart.TotalAmount = cart.Quantity * (from i in _db.Products
                                                    where i.ProductId == cart.Productid
                                                    select i.Price).SingleOrDefault();
                _db.Add(cart);
                await _db.SaveChangesAsync();
                return Ok();

            }
            else
            {
                id.Quantity += cart.Quantity;
                id.TotalAmount = id.Quantity * (from i in _db.Carts
                                                where i.Productid == cart.Productid
                                                select i.Product.Price).SingleOrDefault();
                await _db.SaveChangesAsync();
                return Ok();
            }
            //_context.Add(cart);
            //await _context.SaveChangesAsync();

            //    }

            //ViewData["Productid"] = new SelectList(_context.Product1s, "ProductId", "ProductId", cart.Productid);
            //ViewData["Userid"] = new SelectList(_context.User1s, "UserId", "UserId", cart.Userid);
           

        }
        [HttpGet]
        public async Task<ActionResult<Cart>> Index()
        {
            List<Cart> cart = new List<Cart>();
            //cart = await _db.Carts.Include(p => p.Product).ToListAsync();
            cart = await _db.Carts.ToListAsync();
            return Ok(cart);
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
        [Route("ProceedtoBuy")]
        public async Task<OrderMaster> ProceedtoBuy(int id)
        {
            //var UserId = HttpContext.Session.GetInt32("Userid");
            List<Cart> cart = (from i in _db.Carts where i.Userid == id select i).ToList();
            List<OrderDetail> od = new List<OrderDetail>();

            OrderMaster om = new OrderMaster();

            om.Orderdate = DateTime.Today;
            om.Userid = (int)id;
            om.TotalAmount = 0;
            foreach (var item in cart)
            {

                om.TotalAmount += (int)item.TotalAmount;
            }
            _db.Add(om);

            _db.SaveChanges();
            //HttpContext.Session.SetInt32("Total", (int)om.TotalAmount);
            foreach (var item in cart)
            {
                OrderDetail detail = new OrderDetail();
                detail.Productid = item.Productid;
                detail.Userid = item.Userid;
                detail.TotalAmount = item.TotalAmount;
                detail.OrderMasterid = om.OrderMasterid;
                od.Add(detail);
            }
            _db.AddRange(od);
            _db.SaveChanges();

            return om;

            //return RedirectToAction("GetPayment", new { id = om.OrderMasterid });

        }
        [HttpGet]
        [Route("GetPaymentById")]
        public async Task<OrderMaster> GetPaymentById(int id)
        {
            OrderMaster result = await _db.OrderMasters.Include(x => x.User).Where(x => x.OrderMasterid == id).Select(x => x).FirstOrDefaultAsync();
            return result;

            //var OrderMaster = _context.OrderMasters.Find(id);
            //return View(OrderMaster);
        }
    }
}
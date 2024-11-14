using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Models;
using BookStoreAPI.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace BookLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("AddToCart")]
        public IActionResult AddToCart([FromBody] AddToCartRequest addToCartRequest)
        {
            int quantity = 1;

            var book = _context.Books.FirstOrDefault(b => b.Id == addToCartRequest.BookId);
            if (book == null)
            {
                return NotFound("Kitap bulunamadı.");
            }

            //Tek bir sepette çalışma için kullanılıyor.
            var cart = _context.Carts.FirstOrDefault(); 
            if (cart == null)
            {
                cart = new Cart();
                _context.Carts.Add(cart);
                _context.SaveChanges(); 
            }

            var cartItem = new CartItem
            {
                BookId = addToCartRequest.BookId,
                Quantity = quantity,
                CartId = cart.Id
            };

            _context.CartItems.Add(cartItem);
            _context.SaveChanges();

            return Ok("Kitap sepete eklendi.");
        }

        [HttpGet("GetCartItems")]
        public IActionResult GetCartItems()
        {
            var cartItems = _context.CartItems
                .Include(ci => ci.Book) 
                .Select(ci => new
                {
                    BookId = ci.BookId,
                    BookName = ci.Book.Title,
                    Price = ci.Book.Price,
                    Quantity = ci.Quantity,
                    Total = ci.Quantity * ci.Book.Price
                })
                .ToList();

            return Ok(cartItems);
        }
    }
}

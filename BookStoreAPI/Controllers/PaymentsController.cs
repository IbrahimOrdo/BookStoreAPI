using BookStoreAPI.Models;
using BookStoreAPI.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }        


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            var x = await _context.Payments.ToListAsync();
            return x;
        }

        [HttpPost("ProcessPayment")]
        public IActionResult ProcessPayment([FromBody] ProcessPaymentsRequest processPaymentsRequest)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Burada müşteri bilgileri statik ve belirli bir müşteriye ait olacak şekilde tanımlanmıştır.
                    var order = new Order
                    {
                        CustomerName = "İbrahim",
                        CustomerEmail = "test@test.com",
                        OrderDate = DateTime.Now,
                        CartId = 1,
                    };
                   
                    _context.Orders.Add(order);
                    _context.SaveChanges(); 

                    var cartItems = _context.CartItems
                        .Include(ci => ci.Book)
                        .Where(ci => ci.CartId == 1)
                        .ToList();

                    if (cartItems == null || cartItems.Count == 0)
                    {
                        return NotFound("Sepet boş veya bulunamadı.");
                    }

                    decimal totalAmount = cartItems.Sum(ci => ci.Quantity * ci.Book.Price);

                    var payment = new Payment
                    {
                        OrderId = order.Id, 
                        CardNumber = processPaymentsRequest.cardNumber,
                        CardHolderName = processPaymentsRequest.cardHolderName,
                        ExpirationDate = processPaymentsRequest.expirationDate,
                        CVV = processPaymentsRequest.cvv,
                        Amount = totalAmount
                    };

                    _context.Payments.Add(payment);
                    _context.SaveChanges(); 

                    transaction.Commit();

                    return Ok("Sipariş ve ödeme başarıyla kaydedildi.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, $"İşlem sırasında bir hata oluştu: {ex.Message}");
                }
            }
        }


    }
}

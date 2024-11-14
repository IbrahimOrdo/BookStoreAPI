using Microsoft.AspNetCore.Mvc;
//using BookStoreAPI.Data;
using BookStoreAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var x = await _context.Books.ToListAsync();
            return x;
        }

        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBooks), new { id = book.Id }, book);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using LibraryReservationSystem.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryReservationSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        // GET /api/books
        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetBooks()
        {
            var books = _context.Books.ToList();
            return Ok(books);
        }
    }
}
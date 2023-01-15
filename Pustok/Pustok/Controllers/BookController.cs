using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Models;
using Pustok.ViewModels;

namespace Pustok.Controllers
{
    public class BookController : Controller
    {
        public DataContext _dataContext { get; }

        public BookController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int id)
        {
            Book book = _dataContext.Books
                            .Include(x=>x.Author)
                            .Include(x=>x.BookImage)
                            .Include(x=>x.Genre)
                            .FirstOrDefault(x => x.Id == id);
            if (book is null) return NotFound();
            BookDetailViewModel bookDetailVM = new BookDetailViewModel
            {
                Book = book,
                RelatedBooks = _dataContext.Books
                                .Include(x=>x.Author)
                                .Include(x=>x.BookImage)
                                .Include(x=>x.Genre)
                                .Where(x => x.GenreId == book.GenreId && x.Id !=book.Id)
                                .ToList(),
            };

            return View(bookDetailVM);
        }
    }
}

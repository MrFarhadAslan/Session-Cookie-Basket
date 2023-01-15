using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Helpers;
using Pustok.Models;
using System.IO;
using System.Linq;

namespace Pustok.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BookController : Controller
    {
        private DataContext _dataContext { get; }

        private IWebHostEnvironment _env;

        public BookController(DataContext dataContext, IWebHostEnvironment env)
        {
            _dataContext = dataContext;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Book> books = _dataContext.Books.ToList();
            return View(books);
        }

        public IActionResult Create()
        {
            ViewBag.Authors = _dataContext.Authors.ToList();
            ViewBag.Genres = _dataContext.Genres.ToList();



            return View();
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            ViewBag.Authors = _dataContext.Authors.ToList();
            ViewBag.Genres = _dataContext.Genres.ToList();

            if (!ModelState.IsValid) return View();

            if(book.PosterImageFile != null)
            {
                if (book.PosterImageFile.ContentType != "image/png" && book.PosterImageFile.ContentType != "image/jpg" && book.PosterImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile1", "Yalniz shekil fayli ola biler!");
                    return View();
                }

                if (book.PosterImageFile.Length > 3145728)
                {
                    ModelState.AddModelError("ImageFile1", "Sheklin olcusu max 3 mb ola biler!");
                    return View();
                }

                BookImage bookImage = new BookImage
                {
                    Book = book,
                    Image = FileManager.SaveFile(_env.WebRootPath, "uploads/books", book.PosterImageFile),
                    IsPoster = true
                };

                _dataContext.BookImages.Add(bookImage);

            }

            if(book.HoverImageFile != null)
            {
                if (book.HoverImageFile.ContentType != "image/png" && book.HoverImageFile.ContentType != "image/jpg" && book.HoverImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile1", "Yalniz shekil fayli ola biler!");
                    return View();
                }

                if (book.HoverImageFile.Length > 3145728)
                {
                    ModelState.AddModelError("ImageFile1", "Sheklin olcusu max 3 mb ola biler!");
                    return View();
                }

                BookImage bookImage = new BookImage
                {
                    Book = book,
                    Image = FileManager.SaveFile(_env.WebRootPath, "uploads/books", book.HoverImageFile),
                    IsPoster = false
                };

                _dataContext.BookImages.Add(bookImage);
            }


            if(book.ImageFiles != null)
            {
                foreach (var imageFile in book.ImageFiles)
                {
                    if (imageFile.ContentType != "image/png" && imageFile.ContentType != "image/jpg" && imageFile.ContentType != "image/jpeg")
                    {
                        ModelState.AddModelError("ImageFile1", "Yalniz shekil fayli ola biler!");
                        return View();
                    }

                    if (imageFile.Length > 3145728)
                    {
                        ModelState.AddModelError("ImageFile1", "Sheklin olcusu max 3 mb ola biler!");
                        return View();
                    }

                    BookImage bookImage = new BookImage
                    {
                        Book = book,
                        Image = FileManager.SaveFile(_env.WebRootPath, "uploads/books", imageFile),
                        IsPoster = null
                    };

                    _dataContext.BookImages.Add(bookImage);
                }
            }

            _dataContext.Books.Add(book);
            _dataContext.SaveChanges();
            return RedirectToAction("index");
        }

        public IActionResult Update(int id)
        {
            ViewBag.Authors = _dataContext.Authors.ToList();
            ViewBag.Genres = _dataContext.Genres.ToList();
            Book book = _dataContext.Books.Include(x=>x.BookImage).FirstOrDefault(x=>x.Id ==id);
            ViewBag.BookImages = _dataContext.BookImages.Where(x => x.BookId == book.Id && x.IsPoster ==null).ToList();
            ViewBag.PosterImage = _dataContext.BookImages.FirstOrDefault(x => x.BookId == book.Id && x.IsPoster == true);
            ViewBag.HoverImage = _dataContext.BookImages.FirstOrDefault(x => x.BookId == book.Id && x.IsPoster == false);

            //List<Book> books = _dataContext.Books.Include(x=>x.BookImages).ToList();

            if (book == null) return View("Error");

            return View(book);  

        }

        [HttpPost]
        public IActionResult Update(Book book)
        {
            ViewBag.Authors = _dataContext.Authors.ToList();
            ViewBag.Genres = _dataContext.Genres.ToList();

            Book existBook = _dataContext.Books.Include(x=>x.BookImage).FirstOrDefault(x=>x.Id ==book.Id);

            if(existBook == null) return View("Error");

            if (!ModelState.IsValid) return View(existBook);

            existBook.BookImage.RemoveAll(bi => !book.BookImageIds.Contains(bi.Id) && bi.IsPoster == null);

            ViewBag.BookImages = _dataContext.BookImages.Where(x=>x.BookId == book.Id && x.IsPoster == null).ToList();
            ViewBag.PosterImage = _dataContext.BookImages.FirstOrDefault(x => x.BookId == book.Id && x.IsPoster == true);
            ViewBag.HoverImage = _dataContext.BookImages.FirstOrDefault(x => x.BookId == book.Id && x.IsPoster == false);

            if (book.PosterImageFile != null)
            {
                if (book.PosterImageFile.ContentType != "image/png" && book.PosterImageFile.ContentType != "image/jpg" && book.PosterImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("PosterImageFile", "Yalniz shekil fayli ola biler!");
                    return View(existBook);
                }

                if (book.PosterImageFile.Length > 3145728)
                {
                    ModelState.AddModelError("PosterImageFile", "Sheklin olcusu max 3 mb ola biler!");
                    return View(existBook);
                }

                BookImage bookImage = new BookImage
                {
                    Book = book,
                    Image = FileManager.SaveFile(_env.WebRootPath, "uploads\\books", book.PosterImageFile),
                    IsPoster = true
                };
                //var image = _dataContext.BookImages.FirstOrDefault(x => x.BookId == existBook.Id);

                var image = existBook.BookImage.FirstOrDefault(x => x.IsPoster == true);

                if(image is not null) _dataContext.Remove(image);


                FileManager.DeleteFile(_env.WebRootPath, "uploads\\books", image.Image);

                existBook.BookImage.Add(bookImage);

            }

            if (book.HoverImageFile != null)
            {
                if (book.HoverImageFile.ContentType != "image/png" && book.HoverImageFile.ContentType != "image/jpg" && book.HoverImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("HoverImageFile", "Yalniz shekil fayli ola biler!");
                    return View(existBook);
                }

                if (book.HoverImageFile.Length > 3145728)
                {
                    ModelState.AddModelError("HoverImageFile", "Sheklin olcusu max 3 mb ola biler!");
                    return View(existBook);
                }

                BookImage bookImage = new BookImage
                {
                    BookId = book.Id,
                    Image = FileManager.SaveFile(_env.WebRootPath, "uploads\\books", book.HoverImageFile),
                    IsPoster = false
                };

                //var imageHover = _dataContext.BookImages.FirstOrDefault(x=>x.Book.Id == existBook.Id); 

                var imageHover = existBook.BookImage.FirstOrDefault(x => x.IsPoster == false);

                if (imageHover is not null) _dataContext.Remove(imageHover);
                FileManager.DeleteFile(_env.WebRootPath, "uploads\\books", imageHover.Image);


                existBook.BookImage.Add(bookImage);

            }


            if (book.ImageFiles != null)
            {
                foreach (var imageFile in book.ImageFiles)
                {
                    if (imageFile.ContentType != "image/png" && imageFile.ContentType != "image/jpg" && imageFile.ContentType != "image/jpeg")
                    {
                        ModelState.AddModelError("ImageFiles", "Yalniz shekil fayli ola biler!");
                        return View(existBook);
                    }

                    if (imageFile.Length > 3145728)
                    {
                        ModelState.AddModelError("ImageFiles", "Sheklin olcusu max 3 mb ola biler!");
                        return View(existBook);
                    }

                    BookImage bookImage = new BookImage
                    {
                        BookId = book.Id,
                        Image = FileManager.SaveFile(_env.WebRootPath, "uploads/books", imageFile),
                        IsPoster = null
                    };

                    existBook.BookImage.Add(bookImage);


                }
            }


            existBook.Desc = book.Desc;
            existBook.CostPrice = book.CostPrice;
            existBook.DiscountPrice = book.DiscountPrice;
            existBook.SellPrice = book.SellPrice;
            existBook.Code = book.Code;
            existBook.Name = book.Name;
            existBook.IsAviable = book.IsAviable;
            existBook.IsFeatured = book.IsFeatured;
            existBook.IsNew = book.IsNew;


            _dataContext.SaveChanges();
            return RedirectToAction("index");

        }

        public IActionResult Delete(int id)
        {
            Book book = _dataContext.Books.Find(id);

            if (book is null) return View("Error");

            List<BookImage> list = _dataContext.BookImages.Where(x => x.BookId == id).ToList();

            foreach (var image in list)
            {
                _dataContext.BookImages.Remove(image);
                string name = image.Image;
                FileManager.DeleteFile(_env.WebRootPath,"uploads/books",name);
            }
            _dataContext.Books.Remove(book);
            _dataContext.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult DeleteImage(int id)
        {
            BookImage image = _dataContext.BookImages.Find(id);

            _dataContext.Remove(image);
            _dataContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
            //[HttpPost]
            //public IActionResult Create(Book book)
            //{
            //    ViewBag.Authors = _dataContext.Authors.ToList();
            //    ViewBag.Genres = _dataContext.Genres.ToList();

            //    if (!ModelState.IsValid) return View();

            //    if(book.ImageFiles != null)
            //    {
            //        foreach (var ImageFile in book.ImageFiles)
            //        {
            //            if (ImageFile.ContentType != "image/png" && ImageFile.ContentType != "image/jpg" && ImageFile.ContentType != "image/jpeg")
            //            {
            //                ModelState.AddModelError("ImageFile1", "Yalniz shekil fayli ola biler!");
            //                return View();
            //            }

            //            if (ImageFile.Length > 3145728)
            //            {
            //                ModelState.AddModelError("ImageFile1", "Sheklin olcusu max 3 mb ola biler!");
            //                return View();
            //            }

            //            BookImage bookImage = new BookImage
            //            {
            //                Image = FileManager.SaveFile(_env.WebRootPath, "uploads/books", ImageFile),
            //                IsPoster = null,
            //                Book = book
            //            };
            //        }
            //    }



            //    _dataContext.Books.Add(book);
            //    _dataContext.SaveChanges();

            //    return RedirectToAction(nameof(Index));
            //}


            //public IActionResult Update(int id)
            //{
            //    ViewBag.Authors = _dataContext.Authors.ToList();
            //    ViewBag.Genres = _dataContext.Genres.ToList();

            //    Book book = _dataContext.Books.Find(id);

            //    if(book == null) return View("Error");


            //    return View(book);
            //}

            //[HttpPost]
            //public IActionResult Update(Book book)
            //{
            //    ViewBag.Authors = _dataContext.Authors.ToList();
            //    ViewBag.Genres = _dataContext.Genres.ToList();

            //    Book existBook = _dataContext.Books.Find(book.Id);



            //    return RedirectToAction(nameof(Index));
            //}

            //public IActionResult Delete(int id)
            //{
            //    Book book = _dataContext.Books.Find(id);

            //    if (book == null) return View("Error");

            //    _dataContext.Books.Remove(book);
            //    _dataContext.SaveChanges();

            //    return RedirectToAction(nameof(Index));
            //}
        }
}

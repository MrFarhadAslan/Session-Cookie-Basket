using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.ContentModel;
using Pustok.Models;
using Pustok.ViewModels;
using System.Diagnostics;

namespace Pustok.Controllers
{
    public class HomeController : Controller
    {
        private DataContext _dataContext { get; }
        public HomeController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel
            {
                Sliders = _dataContext.Sliders.OrderBy(x=>x.Order).ToList(),
                Features = _dataContext.Features.ToList(),
                Brands = _dataContext.Brands.ToList(),
                Books = _dataContext.Books.Include(c=>c.Genre).Include(v=>v.Author).ToList(),
                FeaturedBooks = _dataContext.Books.Include(x=>x.BookImage).Include(x=>x.Author).Where(x=>x.IsFeatured).ToList(),
                NewBooks = _dataContext.Books.Include(x=>x.BookImage).Include(x=>x.Author).Where(x=>x.IsNew).ToList(),
                DiscountBooks = _dataContext.Books.Include(x=>x.BookImage).Include(x=>x.Author).Where(x=>x.DiscountPrice > 0).ToList(),
            };

            return View(homeViewModel);
        }


        public IActionResult AddToBasket(int id)
        {
            if (!_dataContext.Books.Any(x => x.Id == id)) return NotFound();
    
            List<BasketViewModel> basketItems= new List<BasketViewModel>();
            BasketViewModel basketItem = null;
            string BasketStr = HttpContext.Request.Cookies["Basket"];

            if (BasketStr != null)
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketViewModel>>(BasketStr);

                basketItem = basketItems.FirstOrDefault(x => x.BookId == id);

                if (basketItem != null) basketItem.Count++;
                else
                {
                    basketItem = new BasketViewModel
                    {
                        BookId = id,
                        Count = 1
                    };
                    basketItems.Add(basketItem);
                }
                

            }
            else
            {
                basketItem = new BasketViewModel
                {
                    BookId = id,
                    Count = 1
                };

                basketItems.Add(basketItem);
            }


            string basketItemsStr = JsonConvert.SerializeObject(basketItems);

            HttpContext.Response.Cookies.Append("Basket", basketItemsStr);

            return Ok();
        }

        public IActionResult GetBasket()
        {
            List<BasketViewModel> Basket = new List<BasketViewModel>();

            string basketItems = HttpContext.Request.Cookies["Basket"];

            if(basketItems != null)
            {
                
                Basket = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketItems);

            }


            return Json(Basket);
        }

        public IActionResult Checkout()
        {
            List<BasketViewModel> basketItems = new List<BasketViewModel>();
            List<CheckoutItemViewModel> checkoutItems = new List<CheckoutItemViewModel>();
            CheckoutItemViewModel checkoutItem = null;

            string basketItemsStr = HttpContext.Request.Cookies["Basket"];

            if(basketItemsStr != null)
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketItemsStr);
                foreach (var item in basketItems)
                {
                    checkoutItem = new CheckoutItemViewModel
                    {
                        Book = _dataContext.Books.FirstOrDefault(x=>x.Id == item.BookId),
                        Count = item.Count
                    };
                    checkoutItems.Add(checkoutItem);
                }
            }

           
            return View(checkoutItems);
        }


        public IActionResult DeleteItemToBasket(int id)
        {
            List<BasketViewModel> basketItems = new List<BasketViewModel>();

            string basketItemsStr = HttpContext.Request.Cookies["Basket"];

            if(basketItemsStr != null)
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketItemsStr);

                var findItem = basketItems.FirstOrDefault(x => x.BookId == id);

                if (findItem == null) return NotFound();
                else
                {
                    if(findItem.Count > 1)
                    {
                        findItem.Count--;
                    }
                    else
                    {
                        basketItems.Remove(findItem);
                    }
                }

            }

            string basketStr = JsonConvert.SerializeObject(basketItems);

            HttpContext.Response.Cookies.Append("Basket",basketStr);
            
            return Ok();
        }





        //public IActionResult SetCookie(int id)
        //{
        //    List<int> bookIds = new List<int>();
            

        //    string check = HttpContext.Request.Cookies["BookName"];

        //    if(check != null)
        //    {
        //        bookIds = JsonConvert.DeserializeObject<List<int>>(check);

        //        bookIds.Add(id);

        //    }
        //    else
        //    {
        //        bookIds.Add(id);
        //    }


        //    string bookIdStr = JsonConvert.SerializeObject(bookIds);


        //    HttpContext.Response.Cookies.Append("BookName", bookIdStr);

        //    return Content("Added Cookie");
        //}

        //public IActionResult GetCookie()
        //{
        //    List<int> bookIds= new List<int>();

        //    string name =  HttpContext.Request.Cookies["BookName"];
        //    if (name == null) return Json(bookIds);

        //    bookIds = JsonConvert.DeserializeObject<List<int>>(name);
           
        //    return Json(bookIds);
        //}
    }
}
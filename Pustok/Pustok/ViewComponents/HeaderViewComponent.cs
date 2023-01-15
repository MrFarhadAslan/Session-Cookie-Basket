using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using Pustok.Models;
using Pustok.ViewModels;
using System.ComponentModel;

namespace Pustok.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        public DataContext _dataContext { get; }
        public HeaderViewComponent(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //Book book = _dataContext.Books.FirstOrDefault();
            //List<CheckoutItemViewModel> checkoutItems= new List<CheckoutItemViewModel>();
            //List<BasketViewModel> basketItems = new List<BasketViewModel>();
            //CheckoutItemViewModel checkoutItem = null;
            //string basketItemsStr = HttpContext.Request.Cookies["Basket"];

            //if(basketItemsStr != null )
            //{
            //    basketItems = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketItemsStr);
            //    foreach (var item in basketItems)
            //    {
            //        checkoutItem = new CheckoutItemViewModel
            //        {
            //            Book = _dataContext.Books.FirstOrDefault(x=>x.Id ==item.BookId ),
            //            Count = item.Count
            //        };

            //    }
            //    checkoutItems.Add(checkoutItem);
            //}
            List<BasketViewModel> basketItems = new List<BasketViewModel>();
            List<CheckoutItemViewModel> checkoutItems = new List<CheckoutItemViewModel>();
            CheckoutItemViewModel checkoutItem = null;

            string basketItemsStr = HttpContext.Request.Cookies["Basket"];

            if (basketItemsStr != null)
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketItemsStr);
                foreach (var item in basketItems)
                {
                    checkoutItem = new CheckoutItemViewModel
                    {
                        Book = _dataContext.Books.FirstOrDefault(x => x.Id == item.BookId),
                        Count = item.Count
                    };
                    checkoutItems.Add(checkoutItem);
                }
            }

            return View(await Task.FromResult(checkoutItems));
        }

    }
}

using Pustok.Models;

namespace Pustok.ViewModels
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; }
        public List<Feature> Features { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Book> Books { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Author> Authors { get; set; }
        public List<Book> FeaturedBooks { get; set; }
        public List<Book> NewBooks { get; set; }
        public List<Book> DiscountBooks { get; set; }
    }
}

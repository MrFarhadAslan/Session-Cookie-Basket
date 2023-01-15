using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok.Models
{
    public class Book
    {
        public int Id { get; set; }

        public int GenreId { get; set; }
        public int AuthorId { get; set; }
        [StringLength(maximumLength:30)]
        public string Name { get; set; }

        [StringLength(maximumLength:250)]
        public string Desc { get; set; }
        public double CostPrice { get; set; }
        public double SellPrice { get; set; }
        public double DiscountPrice { get; set; }

        [StringLength(maximumLength:20)]
        public string Code { get; set; }
        public bool IsAviable { get; set; }
        
        public bool IsFeatured { get; set; }
        public bool IsNew { get; set; }
        public Genre? Genre { get; set; }
        public Author? Author { get; set; }

        //[NotMapped]
        //public IFormFile ImageFile1 { get; set; }

        //[NotMapped]
        //public IFormFile ImageFile2 { get; set; }
        public List<BookImage>? BookImage { get; set;}

        [NotMapped]
        public List<IFormFile>? ImageFiles { get; set; }
        [NotMapped]
        public IFormFile? PosterImageFile { get; set; }
        [NotMapped]
        public IFormFile? HoverImageFile { get; set; }
        [NotMapped]
        public List<int>? BookImageIds { get; set; }
    }
}

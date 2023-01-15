using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Pustok.Models
{
    public class Feature
    {
        public int Id { get; set; }
        [StringLength(maximumLength:30)] 
        public string Name { get; set; }

        [StringLength(maximumLength:30)] 
        public string Desc { get; set; }
        public string Icon { get; set; }
       
    }
}

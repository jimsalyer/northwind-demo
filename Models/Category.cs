using System.ComponentModel.DataAnnotations;
using Sieve.Attributes;

namespace NorthwindDemo.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [StringLength(15)]
        [Sieve(CanFilter = true, CanSort = true)]
        public string CategoryName { get; set; }

        public string Description { get; set; }

        public byte[] Picture { get; set; }
    }
}

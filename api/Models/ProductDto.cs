using System.ComponentModel.DataAnnotations;
using Sieve.Attributes;

namespace NorthwindApi.Models
{
    public class ProductDto
    {
        public int SupplierId { get; set; }

        public int CategoryId { get; set; }

        [Required]
        [StringLength(40)]
        [Sieve(CanFilter = true, CanSort = true)]
        public string ProductName { get; set; }

        [Required]
        [StringLength(20)]
        [Sieve(CanFilter = true)]
        public string QuantityPerUnit { get; set; }

        [Range(0, double.MaxValue)]
        [Sieve(CanFilter = true, CanSort = true)]
        public double UnitPrice { get; set; }

        [Range(0, int.MaxValue)]
        [Sieve(CanFilter = true, CanSort = true)]
        public int UnitsInStock { get; set; }

        [Range(0, int.MaxValue)]
        [Sieve(CanFilter = true, CanSort = true)]
        public int UnitsOnOrder { get; set; }

        [Range(0, int.MaxValue)]
        [Sieve(CanFilter = true, CanSort = true)]
        public int ReorderLevel { get; set; }

        [Range(0, 1)]
        [Sieve(CanFilter = true)]
        public int Discontinued { get; set; }
    }
}

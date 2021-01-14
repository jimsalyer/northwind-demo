using System.ComponentModel.DataAnnotations;
using Sieve.Attributes;

namespace NorthwindDemo.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public int SupplierId { get; set; }

        public int CategoryId { get; set; }

        [Required]
        [StringLength(40)]
        [Sieve(CanSort = true)]
        public string ProductName { get; set; }

        [Required]
        [StringLength(20)]
        public string QuantityPerUnit { get; set; }

        [Range(0, double.MaxValue)]
        [Sieve(CanSort = true)]
        public double UnitPrice { get; set; }

        [Range(0, int.MaxValue)]
        [Sieve(CanSort = true)]
        public int UnitsInStock { get; set; }

        [Range(0, int.MaxValue)]
        [Sieve(CanSort = true)]
        public int UnitsOnOrder { get; set; }

        [Range(0, int.MaxValue)]
        [Sieve(CanSort = true)]
        public int ReorderLevel { get; set; }

        [Range(0, 1)]
        public int Discontinued { get; set; }
    }
}

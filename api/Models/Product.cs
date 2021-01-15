using AutoMapper;

namespace NorthwindApi.Models
{
    [AutoMap(typeof(ProductDto))]
    public class Product
    {
        public int ProductId { get; set; }

        public int SupplierId { get; set; }

        public int CategoryId { get; set; }

        public string ProductName { get; set; }

        public string QuantityPerUnit { get; set; }

        public double UnitPrice { get; set; }

        public int UnitsInStock { get; set; }

        public int UnitsOnOrder { get; set; }

        public int ReorderLevel { get; set; }

        public int Discontinued { get; set; }
    }
}

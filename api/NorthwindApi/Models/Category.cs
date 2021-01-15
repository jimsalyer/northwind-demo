using AutoMapper;

namespace NorthwindApi.Models
{
    [AutoMap(typeof(CategoryDto))]
    public class Category
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public byte[] Picture { get; set; }
    }
}

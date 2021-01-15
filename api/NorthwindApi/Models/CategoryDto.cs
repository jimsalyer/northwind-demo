using System.ComponentModel.DataAnnotations;
using Sieve.Attributes;

namespace NorthwindApi.Models
{
    public class CategoryDto
    {
        [Required]
        [StringLength(15)]
        [Sieve(CanFilter = true, CanSort = true)]
        public string CategoryName { get; set; }

        public string Description { get; set; }

        public byte[] Picture { get; set; }

        public CategoryDto Clone()
        {
            return (CategoryDto)MemberwiseClone();
        }
    }
}

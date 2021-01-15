using System.ComponentModel.DataAnnotations;

namespace NorthwindApi.Models
{
    public class SupplierDto
    {
        [Required]
        [StringLength(40)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(30)]
        public string ContactName { get; set; }

        [Required]
        [StringLength(30)]
        public string ContactTitle { get; set; }

        [Required]
        [StringLength(60)]
        public string Address { get; set; }

        [Required]
        [StringLength(16)]
        public string City { get; set; }

        [StringLength(15)]
        public string Region { get; set; }

        [Required]
        [StringLength(10)]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(15)]
        public string Country { get; set; }

        [Required]
        [StringLength(24)]
        public string Phone { get; set; }

        [StringLength(24)]
        public string Fax { get; set; }

        public string Homepage { get; set; }

        public SupplierDto Clone()
        {
            return (SupplierDto)MemberwiseClone();
        }
    }
}

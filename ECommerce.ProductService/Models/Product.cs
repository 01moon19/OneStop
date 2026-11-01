
using System.ComponentModel.DataAnnotations;

namespace ECommerce.ProductService.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal ProductPrice { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProductCategory { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string ProductDescription { get; set; } = string.Empty;

        public string ProductImage { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }
    }
}

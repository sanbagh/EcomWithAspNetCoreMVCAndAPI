using System.ComponentModel.DataAnnotations;

namespace ECommerceDemo.Models
{
    public class BasketItem
    {
        [Required]
        public int Id { get; set; }
        [Required]

        public string Name { get; set; }
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Price should be greater than zero")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Quantity should be greater than zero")]
        public int Quantity { get; set; }
        [Required]
        public string PhotoUrl { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
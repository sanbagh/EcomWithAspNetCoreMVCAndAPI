namespace ECommerceDemo.Models
{
    public class OrderItem
    {
        public int ProductItemId { get; set; }
        public string ProductName { get; set; }
        public string PhotoUrl { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }

    }
}
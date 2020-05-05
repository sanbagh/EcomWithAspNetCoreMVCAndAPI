namespace ECommerceDemo.Models
{
    public class Order
    {
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public Address ShippingAddress { get; set; }

    }

}
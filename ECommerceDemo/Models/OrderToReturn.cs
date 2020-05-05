using System;
using System.Collections.Generic;

namespace ECommerceDemo.Models
{
    public class OrderToReturn
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public IReadOnlyList<OrderItem> OrderedItems { get; set; }
        public decimal SubTotal { get; set; }
        public Address ShippingAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal ShippingPrice { get; set; }
        public decimal Total { get; set; }
        public string OrderStatus { get; set; }
    }
}
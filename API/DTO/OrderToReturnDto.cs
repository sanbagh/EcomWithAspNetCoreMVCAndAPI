using System;
using System.Collections.Generic;
using Core.OrderAggregate;

namespace API.DTO
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public IReadOnlyList<OrderItemDto> OrderedItems { get; set; }
        public decimal SubTotal { get; set; }
        public Address ShippingAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal ShippingPrice { get; set; }
        public decimal Total { get; set; }
        public string OrderStatus { get; set; }
    }
}
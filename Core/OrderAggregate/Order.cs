using System;
using System.Collections.Generic;
using Core.Entities;

namespace Core.OrderAggregate
{
    public class Order : BaseEntity
    {
        public Order()
        {
        }

        public Order(IReadOnlyList<OrderItem> orderItems, string buyerEmail, decimal subTotal, Address shippingAddress, DeliveryMethod deliveryMethod)
        {
            this.BuyerEmail = buyerEmail;
            this.SubTotal = subTotal;
            this.ShippingAddress = shippingAddress;
            this.DeliveryMethod = deliveryMethod;
            this.OrderedItems = orderItems;
        }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public IReadOnlyList<OrderItem> OrderedItems { get; set; }
        public decimal SubTotal { get; set; }
        public Address ShippingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public string PaymentIntentId { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public decimal GetTotal()
        {
            return SubTotal + DeliveryMethod.Price;
        }

    }
}
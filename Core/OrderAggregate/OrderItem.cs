using Core.Entities;

namespace Core.OrderAggregate
{
    public class OrderItem : BaseEntity
    {
        public OrderItem()
        {
        }

        public OrderItem(ProductItemOrdered itemOrdered, decimal price, decimal quantity)
        {
            this.ItemOrdered = itemOrdered;
            this.Price = price;
            this.Quantity = quantity;

        }
        public ProductItemOrdered ItemOrdered { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
    }
}
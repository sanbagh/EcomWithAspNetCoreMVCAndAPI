using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.OrderAggregate;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepo _bskrepo;
        private readonly IUnitOfWork _uow;
        public OrderService(IBasketRepo bskrepo, IUnitOfWork uow)
        {
            _uow = uow;
            _bskrepo = bskrepo;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            var basket = await _bskrepo.GetBasketAsync(basketId);
            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await _uow.Repository<Product>().GetByIdAsync(item.Id);
                var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PhotoUrl);
                orderItems.Add(new OrderItem(productItemOrdered, product.Price, item.Quantity));
            }
            var order = new Order(orderItems,
                buyerEmail, orderItems.Sum(x => x.Price * x.Quantity), shippingAddress,
                await _uow.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId));
            _uow.Repository<Order>().Add(order);
            var result = await _uow.Complete();

            if (result <= 0) return null;
            await _bskrepo.DeleteBasketAsync(basketId);

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _uow.Repository<DeliveryMethod>().GetAllAsync();
        }

        public async Task<Order> GetOrdersByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrderWithOrderItemAndOrderingSpec(id, buyerEmail);
            return await _uow.Repository<Order>().GetEntityBySpecAsync(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderWithOrderItemAndOrderingSpec(buyerEmail);
            return await _uow.Repository<Order>().GetAllBySpecAsync(spec);
        }
    }
}
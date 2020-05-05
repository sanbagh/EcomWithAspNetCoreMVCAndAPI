using API.DTO;
using AutoMapper;
using Core.Entities;
using Core.OrderAggregate;
using Microsoft.Extensions.Configuration;

namespace API.Helpers
{
    public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _config;
        public OrderItemUrlResolver(IConfiguration config)
        {
            _config = config;

        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(source.ItemOrdered.PhotoUrl)) return null;

            return _config["ApiUrl"] + source.ItemOrdered.PhotoUrl;
        }
    }
}